using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class A0002_LoginHandler : AMRpcHandler<A0002_Login_C2R, A0002_Login_R2C>
    {
        protected override async ETTask Run(Session session, A0002_Login_C2R request, A0002_Login_R2C response, Action reply)
        {
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                //验证提交来的的账号和密码
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}',Password:'{request.Password}'}}");

                if (result.Count != 1)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply();
                    return;
                }

                //已验证通过，可能存在其它地方有登录，要先踢下线
                AccountInfo account = (AccountInfo)result[0];
                await RealmHelper.KickOutPlayer(account.Id);

                int GateAppId;
                StartConfig config;
                //获取账号所在区服的AppId 索取登陆Key
                if (StartConfigComponent.Instance.GateConfigs.Count == 1)
                { //只有一个Gate服务器时当作AllServer配置处理
                    config = StartConfigComponent.Instance.StartConfig;
                }
                else
                { //有多个Gate服务器时当作分布式配置处理
                    GateAppId = RealmHelper.GetGateAppIdFromUserId(account.Id);
                    config = StartConfigComponent.Instance.GateConfigs[GateAppId - 1];
                }
                IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
                string outerAddress = config.GetComponent<OuterConfig>().Address2;

                A0006_GetLoginKey_G2R g2RGetLoginKey = (A0006_GetLoginKey_G2R)await gateSession.Call(new A0006_GetLoginKey_R2G() { UserID = account.Id });

                response.GateAddress = outerAddress;
                response.GateLoginKey = g2RGetLoginKey.GateLoginKey;
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}