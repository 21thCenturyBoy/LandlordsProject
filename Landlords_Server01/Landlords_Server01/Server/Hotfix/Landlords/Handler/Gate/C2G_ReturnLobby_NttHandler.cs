using ETModel;
using System.Net;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_ReturnLobby_NttHandler : AMHandler<C2G_ReturnLobby_Ntt>
    {
        protected override async ETTask Run(Session session, C2G_ReturnLobby_Ntt message)
        {
            //验证Session
            if (!GateHelper.SignSession(session))
            {
                return;
            }

            User user = session.GetComponent<SessionUserComponent>().User;
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();

            //通知Map服务器玩家离开房间
            if (user.ActorID != 0)
            {
                ActorMessageSender actorProxy = actorProxyComponent.Get(user.ActorID);
                actorProxy.Send(new Actor_PlayerExitRoom_G2M());

                user.ActorID = 0;
            }
            await ETTask.CompletedTask;
        }
    }
}