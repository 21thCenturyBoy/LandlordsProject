using ETModel;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace ETHotfix
{
    public static class LandMatchComponentSystem
    {
        /// <summary>
        /// 已开始游戏的玩家可以迅速找到自己的房间
        /// </summary>
        public static Room GetGamingRoom(this LandMatchComponent self, Gamer gamer)
        {
            Room room;
            if (!self.Playing.TryGetValue(gamer.UserID, out room))
            {
                Log.Error("玩家不在已经开始游戏的房间中");
            }
            return room;
        }

        /// <summary>
        /// 返回未开始游戏的房间 用于处理准备消息
        /// </summary>
        public static Room GetWaitingRoom(this LandMatchComponent self, Gamer gamer)
        {
            Room room;
            if (!self.Waiting.TryGetValue(gamer.UserID, out room))
            {
                Log.Error("玩家不在待机的房间中");
            }
            return room;
        }

        /// <summary>
        /// 获取一个可以添加座位的房间 没有则返回null
        /// </summary>
        public static Room GetFreeLandlordsRoom(this LandMatchComponent self)
        {
            return self.FreeLandlordsRooms.Where(p => p.Value.Count < 3).FirstOrDefault().Value;
        }

        /// <summary>
        /// 斗地主匹配队列人数加一
        /// 队列模式 所以没有插队/离队操作
        /// 队列满足匹配条件时 创建新房间
        /// </summary>
        public static void AddGamerToMatchingQueue(this LandMatchComponent self, Gamer gamer)
        {
            //添加玩家到队列
            self.MatchingQueue.Enqueue(gamer);
            Log.Debug("一位玩家加入队列");
            //广播通知所有匹配中的玩家
            self.Broadcast(new Actor_LandMatcherPlusOne_NTT() { MatchingNumber = self.MatchingQueue.Count });

            //检查匹配状态
            self.MatchingCheck();
        }

        /// <summary>
        /// 检查匹配状态 每当有新排队玩家加入时执行一次
        /// </summary>
        /// <param name="self"></param>
        public static async void MatchingCheck(this LandMatchComponent self)
        {
            //如果有空房间 且 正在排队的玩家>0
            Room room = self.GetFreeLandlordsRoom();
            if (room != null)
            {
                while (self.MatchingQueue.Count > 0 && room.Count < 3)
                {
                    self.JoinRoom(room, self.MatchingQueue.Dequeue());
                }
            } //else 如果没有空房间 且 正在排队的玩家>=1
            else if (self.MatchingQueue.Count >= 1)
            {
                //创建新房间
                room = ComponentFactory.Create<Room>();
                await room.AddComponent<MailBoxComponent>().AddLocation();
                self.FreeLandlordsRooms.Add(room.Id, room);

                while (self.MatchingQueue.Count > 0 && room.Count < 3)
                {
                    self.JoinRoom(room, self.MatchingQueue.Dequeue());
                }
            }
        }


        /// <summary>
        /// 加入房间
        /// </summary>
        public static void JoinRoom(this LandMatchComponent self, Room room, Gamer gamer)
        {
            //玩家可能掉线
            if (gamer == null)
            {
                return;
            }

            //玩家加入房间 成为已经进入房间的玩家
            //绑定玩家与房间 以后可以通过玩家UserID找到所在房间
            self.Waiting[gamer.UserID] = room;
            //为玩家添加座位 
            room.Add(gamer);
            //房间广播
            Log.Info($"玩家{gamer.UserID}进入房间");
            Actor_GamerEnterRoom_Ntt broadcastMessage = new Actor_GamerEnterRoom_Ntt();
            foreach (Gamer _gamer in room.gamers)
            {
                if (_gamer == null)
                {
                    //添加空位
                    broadcastMessage.Gamers.Add(new GamerInfo());
                    continue;
                }

                //添加玩家信息
                //GamerInfo info = new GamerInfo() { UserID = _gamer.UserID, IsReady = room.IsGamerReady(gamer) };
                GamerInfo info = new GamerInfo() { UserID = _gamer.UserID };
                broadcastMessage.Gamers.Add(info);
            }
            //广播房间内玩家消息 每次有人进入房间都会收到一次广播
            room.Broadcast(broadcastMessage);

            //向Gate上的User发送匹配成功 
            ActorMessageSender actorProxy = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(gamer.GActorID);
            actorProxy.Send(new Actor_MatchSucess_M2G() { GamerID = gamer.InstanceId });
        }


        /// <summary>
        /// 匹配队列广播
        /// </summary>
        public static void Broadcast(this LandMatchComponent self, IActorMessage message)
        {
            foreach (Gamer gamer in self.MatchingQueue)
            {
                //向客户端User发送Actor消息
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorID);

                Log.Debug("转发给了客户端一条消息，客户端Session：" + gamer.CActorID.ToString());
                actorProxy.Send(message);
            }
        }
    }
}