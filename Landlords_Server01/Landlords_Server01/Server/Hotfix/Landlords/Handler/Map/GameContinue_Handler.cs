using System;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GameContinue_Handler : AMActorRpcHandler<Gamer,Actor_GamerContinue_Req, Actor_GamerContinue_Back>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerContinue_Req request, Actor_GamerContinue_Back response, Action reply)
        {
            try
            {
                Room room = Game.Scene.GetComponent<LandMatchComponent>().GetWaitingRoom(gamer);

                //如果消息到达时，正好2分钟到房间已经清除
                if (room == null)
                {
                    //通知客户端房间不存在，重新匹配房间
                    response.Error = ErrorCode.ERR_GameContinueError;
                }
                else
                {
                    room.CancellationTokenSource?.Cancel();
                }
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

        }
    }
}