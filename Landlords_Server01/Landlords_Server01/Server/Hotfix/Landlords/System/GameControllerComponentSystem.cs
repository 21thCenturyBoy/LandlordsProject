using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameControllerComponentAwakeSystem : AwakeSystem<GameControllerComponent, RoomConfig>
    {
        public override void Awake(GameControllerComponent self, RoomConfig config)
        {
            self.Awake(config);
        }
    }

    public static class GameControllerComponentSystem
    {
        public static void Awake(this GameControllerComponent self, RoomConfig config)
        {
            self.Config = config;
            self.BasePointPerMatch = config.BasePointPerMatch;
            self.Multiples = config.Multiples;
            self.MinThreshold = config.MinThreshold;
        }

        /// <summary>
        /// 准备开始游戏
        /// </summary>
        /// <param name="self"></param>
        public static void StartGame(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            Gamer[] gamers = room.gamers;

            //初始玩家开始状态
            foreach (var _gamer in gamers)
            {
                if (_gamer.GetComponent<HandCardsComponent>() == null)
                {
                    _gamer.AddComponent<HandCardsComponent>();
                }
            }

            GameControllerComponent gameController = room.GetComponent<GameControllerComponent>();
            //洗牌发牌
            gameController.DealCards();

            List<GamerCardNum> gamersCardNum = new List<GamerCardNum>();
            Array.ForEach(gamers, (g) =>
            {
                HandCardsComponent handCards = g.GetComponent<HandCardsComponent>();
                //重置玩家身份
                handCards.AccessIdentity = Identity.None;
                //重置玩家手牌数
                gamersCardNum.Add(new GamerCardNum()
                {
                    UserID = g.UserID,
                    Num = g.GetComponent<HandCardsComponent>().GetAll().Length
                });
            });

            //向客户端发送玩家手牌和玩家手牌数
            foreach (var _gamer in gamers)
            {
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                ActorMessageSender actorProxy = actorProxyComponent.Get(_gamer.CActorID);

                actorProxy.Send(new Actor_GameStartHandCards_Ntt()
                {
                    HandCards = To.RepeatedField(_gamer.GetComponent<HandCardsComponent>().GetAll()),
                    GamersCardNum = To.RepeatedField(gamersCardNum)
                });
            }

            //随机先手玩家
            gameController.RandomFirstAuthority();
            Log.Info($"房间{room.Id}开始游戏");
        }

        /// <summary>
        /// 轮流发牌
        /// </summary>
        /// <param name="self"></param>
        public static void DealCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();

            //牌库洗牌
            room.GetComponent<DeckComponent>().Shuffle();

            //玩家轮流发牌
            Gamer[] gamers = room.gamers;
            int index = 0;
            for (int i = 0; i < 51; i++)
            {
                if (index == 3)
                {
                    index = 0;
                }
                self.DealTo(gamers[index].UserID);
                index++;
            }

            //发地主牌
            for (int i = 0; i < 3; i++)
            {
                self.DealToLord(room.Id);
            }
            self.Multiples = self.Config.Multiples;
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <param name="id"></param>
        public static void DealTo(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            Card card = room.GetComponent<DeckComponent>().Deal();

            foreach (var gamer in room.gamers)
            {
                if (id == gamer.UserID)
                {
                    gamer.GetComponent<HandCardsComponent>().AddCard(card);
                    break;
                }
            }

        }
        /// <summary>
        /// 随机先手玩家
        /// </summary>
        public static void RandomFirstAuthority(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
            Gamer[] gamers = room.gamers;

            int index = RandomHelper.RandomNumber(0, gamers.Length);
            long firstAuthority = gamers[index].UserID;
            orderController.Init(firstAuthority);

            //广播先手抢地主玩家
            room.Broadcast(new Actor_AuthorityGrabLandlord_Ntt() { UserID = firstAuthority });
        }

        /// <summary>
        /// 暂用存地主牌方法
        /// </summary>
        public static void DealToLord(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            Card card = room.GetComponent<DeckComponent>().Deal();
            if (id == room.Id)
            {
                room.LordCache.Add(card);
                room.LordCards.Add(card);
            }
        }
        /// <summary>
        /// 暂用发地主牌方法
        /// </summary>
        public static Card DealLord(this GameControllerComponent self, Room room)
        {
            Card card = room.LordCache[room.LordCache.Count - 1];
            room.LordCache.Remove(card);
            return card;
        }

        /// <summary>
        /// 向客户端发地主牌
        /// </summary>
        public static void CardsOnTable(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
            HandCardsComponent handCards = room.GetGamerFromUserID(id).GetComponent<HandCardsComponent>();

            orderController.Start(id);

            for (int i = 0; i < 3; i++)
            {
                Card card = self.DealLord(room);
                handCards.AddCard(card);
            }

            //更新玩家身份
            foreach (var gamer in room.gamers)
            {
                Identity gamerIdentity = gamer.UserID == id ? Identity.Landlord : Identity.Farmer;
                self.UpdateInIdentity(gamer, gamerIdentity);
            }

            //广播地主消息
            room.Broadcast(new Actor_SetLandlord_Ntt() { UserID = id, LordCards = To.RepeatedField(room.LordCards) });

            //广播地主先手出牌消息
            room.Broadcast(new Actor_AuthorityPlayCard_Ntt() { UserID = id, IsFirst = true });
        }

        /// <summary>
        /// 更新身份
        /// </summary>
        public static void UpdateInIdentity(this GameControllerComponent self, Gamer gamer, Identity identity)
        {
            gamer.GetComponent<HandCardsComponent>().AccessIdentity = identity;
        }

        /// <summary>
        /// 场上的所有牌回到牌库中
        /// </summary>
        public static void BackToDeck(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            DeckComponent deckComponent = room.GetComponent<DeckComponent>();

            //回收玩家手牌
            foreach (var gamer in room.gamers)
            {
                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                while (handCards.CardsCount > 0)
                {
                    Card card = handCards.library[handCards.CardsCount - 1];
                    handCards.PopCard(card);
                    deckComponent.AddCard(card);
                }
            }
        }
    }
}