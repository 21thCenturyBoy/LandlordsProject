using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameStartHandCards_NttHandler : AMHandler<Actor_GameStartHandCards_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GameStartHandCards_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();

            //初始化玩家UI
            foreach (GamerCardNum gamerCardNum in message.GamersCardNum)
            {
                Gamer gamer = room.GetGamer(gamerCardNum.UserID);
                LandlordsGamerPanelComponent gamerUI = gamer.GetComponent<LandlordsGamerPanelComponent>();
                gamerUI.GameStart();

                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                if (handCards != null)
                {
                    handCards.Reset();
                }
                else
                {
                    //Log.Debug("没有可以复用的HandCardsComponent，创建一个。");
                    handCards = gamer.AddComponent<HandCardsComponent, GameObject>(gamerUI.Panel);
                }

                //显示牌背面或者手牌
                handCards.Appear();
                //添加与更新本地玩家的手牌
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    //本地玩家添加手牌
                    Card[] Tcards = new Card[message.HandCards.Count];
                    for (int i = 0; i < message.HandCards.Count; i++)
                    {
                        Tcards[i] = message.HandCards[i];
                    }
                    handCards.AddCards(Tcards);
                }
                else
                {
                    //设置其他玩家手牌数
                    handCards.SetHandCardsNum(gamerCardNum.Num);
                }
            }

            //显示牌桌UI
            GameObject desk = uiRoom.GameObject.Get<GameObject>("Desk");
            desk.SetActive(true);
            GameObject lordPokers = desk.Get<GameObject>("LordPokers");

            //重置地主牌
            Sprite lordSprite = CardHelper.GetCardSprite("None");
            for (int i = 0; i < lordPokers.transform.childCount; i++)
            {
                lordPokers.transform.GetChild(i).GetComponent<Image>().sprite = lordSprite;
            }

            LandRoomComponent uiRoomComponent = uiRoom.GetComponent<LandRoomComponent>();

            //设置初始倍率
            uiRoomComponent.SetMultiples(1);

            await ETTask.CompletedTask;
        }
    }
}