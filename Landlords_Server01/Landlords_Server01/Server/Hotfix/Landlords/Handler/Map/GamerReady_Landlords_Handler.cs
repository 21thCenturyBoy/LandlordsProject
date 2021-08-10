using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GamerReady_Landlords_Handler : AMActorHandler<Gamer, Actor_GamerReady_Landlords>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerReady_Landlords message)
        {
            LandMatchComponent landordsMatchComponent = Game.Scene.GetComponent<LandMatchComponent>();
            //请求的gamer目前所在等待中的房间
            Room room = landordsMatchComponent.GetWaitingRoom(gamer);
            if (room != null)
            {
                //找到玩家的座位顺序
                int seatIndex = room.GetGamerSeat(gamer.UserID);
                if (seatIndex >= 0)
                {
                    //由等待状态设置为准备状态
                    room.isReadys[seatIndex] = true;
                    //广播通知全房间玩家此gamer已经准备好
                    room.Broadcast(new Actor_GamerReady_Landlords() { UserID = gamer.UserID });
                    //检测开始游戏，判断如果房间内三个人都是准备状态就开始游戏和发牌
                    room.CheckGameStart();
                }
                else
                {
                    Log.Error("玩家不在正确的座位上");
                }
            }

            await ETTask.CompletedTask;
        }
    }
}