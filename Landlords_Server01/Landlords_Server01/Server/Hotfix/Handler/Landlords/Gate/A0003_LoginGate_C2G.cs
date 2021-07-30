using System;
using ETModel;
using System.Net;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class A0003_LoginGateHanler : AMRpcHandler<A0003_LoginGate_C2G, A0003_LoginGate_G2C>
    {
        protected override async ETTask Run(Session session, A0003_LoginGate_C2G request, A0003_LoginGate_G2C response, Action reply)
        {
            try
            {
                SessionKeyComponent SessionKeyComponent = Game.Scene.GetComponent<SessionKeyComponent>();
                //获取玩家的永久Id
                long gateUserID = SessionKeyComponent.Get(request.GateLoginKey);

                //验证登录Key是否正确
                if (gateUserID == 0)
                {
                    response.Error = ErrorCode.ERR_ConnectGateKeyError;
                    //客户端提示：连接网关服务器超时
                    reply();
                    return;
                }

                //Key过期
                SessionKeyComponent.Remove(request.GateLoginKey);

                //gateUserID传参创建User
                User user = ComponentFactory.Create<User, long>(gateUserID);

                //将新上线的User添加到UserComponent容器中
                Game.Scene.GetComponent<UserComponent>().Add(user);
                user.AddComponent<MailBoxComponent>();

                //session挂SessionUser组件，user绑定到session上
                //session挂MailBox组件可以通过MailBox进行actor通信
                session.AddComponent<SessionUserComponent>().User = user;
                session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);

                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                //构建realmSession通知Realm服务器 玩家已上线
                //...

                //设置User的参数
                user.GateAppID = config.StartConfig.AppId;
                user.GateSessionID = session.InstanceId;
                user.ActorID = 0;

                //回复客户端
                response.UserID = user.UserID;
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