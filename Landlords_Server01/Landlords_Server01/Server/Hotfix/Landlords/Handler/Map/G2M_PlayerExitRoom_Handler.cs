using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    //Gate通知Map玩家退出房间
    [ActorMessageHandler(AppType.Map)]
    public class G2M_PlayerExitRoom_Handler : AMActorHandler<Gamer, Actor_PlayerExitRoom_G2M>
    {
        protected override async ETTask Run(Gamer gamer, Actor_PlayerExitRoom_G2M message)
        {
            Room room;
            if (Game.Scene.GetComponent<LandMatchComponent>().Playing.TryGetValue(gamer.UserID, out room))
            {
                gamer.isOffline = true;
                //玩家断开添加自动出牌组件
                //...
            }
            else //玩家不在游戏中 处于待机状态
            {
                Game.Scene.GetComponent<LandMatchComponent>().Waiting.TryGetValue(gamer.UserID, out room);
                //房间移除玩家
                room.Remove(gamer.UserID);
                Game.Scene.GetComponent<LandMatchComponent>().Waiting.Remove(gamer.UserID);
                //消息广播给其他人
                room.Broadcast(new Actor_GamerExitRoom_Ntt() { UserID = gamer.UserID });
                gamer.Dispose();
            }
            await ETTask.CompletedTask;
        }
    }
}