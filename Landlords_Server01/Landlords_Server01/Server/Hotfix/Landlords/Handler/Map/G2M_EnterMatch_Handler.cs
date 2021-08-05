using System;
using ETModel;
using System.Threading.Tasks;

namespace ETHotfix
{
    /// <summary>
    /// Gate向Map发送“玩家请求匹配”消息
    /// </summary>
    [MessageHandler(AppType.Map)]
    public class G2M_EnterMatch_Handler : AMHandler<EnterMatchs_G2M>
    {
        protected override async ETTask Run(Session session, EnterMatchs_G2M message)
        {
            //Log.Debug("Map服务器收到第一条消息");
            LandMatchComponent matchComponent = Game.Scene.GetComponent<LandMatchComponent>();

            //玩家是否已经开始游戏
            if (matchComponent.Waiting.ContainsKey(message.UserID))
            {
                Room room;
                matchComponent.Waiting.TryGetValue(message.UserID, out room);
                Gamer gamer = room.GetGamerFromUserID(message.UserID);

                //设置GateActorID，ClientActorID
                gamer.GActorID = message.GActorID;
                gamer.CActorID = message.CActorID;

                //向Gate发送消息更新Gate上user的ActorID
                //这样不论玩家到了哪个地图服务器，Gate上的user都持有所在地图服务器上gamer的InstanceId
                ActorMessageSender actorProxy = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(gamer.GActorID);
                actorProxy.Send(new Actor_MatchSucess_M2G() { GamerID = gamer.InstanceId });
            }
            else
            {
                //新建玩家,用UserID构建Gamer
                Gamer newgamer = ComponentFactory.Create<Gamer, long>(message.UserID);
                newgamer.GActorID = message.GActorID;
                newgamer.CActorID = message.CActorID;

                //为Gamer添加组件
                await newgamer.AddComponent<MailBoxComponent>().AddLocation();

                //添加玩家到匹配队列 广播一遍正在匹配中的玩家
                matchComponent.AddGamerToMatchingQueue(newgamer);
            }

        }
    }
}