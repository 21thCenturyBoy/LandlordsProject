using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class A0006_GetLoginKeyHandler : AMRpcHandler<A0006_GetLoginKey_R2G, A0006_GetLoginKey_G2R>
    {
        protected override async ETTask Run(Session session, A0006_GetLoginKey_R2G request, A0006_GetLoginKey_G2R response, Action reply)
        {
            try
            {
                long key = RandomHelper.RandInt64();
                Game.Scene.GetComponent<SessionKeyComponent>().Add(key, request.UserID);
                response.GateLoginKey = key;
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