using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GameContinue_Handler : AMActorHandler<Gamer, Actor_GamerContinue_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerContinue_Ntt message)
        {
            Room room = Game.Scene.GetComponent<LandMatchComponent>().GetGamingRoom(gamer);

            //如果消息到达时，正好2分钟到房间已经清除
            if (room == null)
            {
                //通知客户端房间不存在，重新匹配房间
                //...
            }
            else
            {
                room.CancellationTokenSource?.Cancel();
            }

            await ETTask.CompletedTask;
        }
    }
}