using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class A0007_KickOutPlayerHandler : AMRpcHandler<A0007_KickOutPlayer_R2G, A0007_KickOutPlayer_G2R>
    {
        protected override async ETTask Run(Session session, A0007_KickOutPlayer_R2G request, A0007_KickOutPlayer_G2R response, Action reply)
        {
            try
            {
                //获取此UserID的网关session
                long sessionId = Game.Scene.GetComponent<UserComponent>().Get(request.UserID).GateSessionID;
                Session lastSession = Game.Scene.GetComponent<NetOuterComponent>().Get(sessionId);

                //移除session与user的绑定
                lastSession.RemoveComponent<SessionUserComponent>();

                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

        }
    }
}