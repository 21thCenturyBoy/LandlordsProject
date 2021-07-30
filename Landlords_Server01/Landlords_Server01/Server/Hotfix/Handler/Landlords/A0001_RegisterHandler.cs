using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class A0001_RegisterHandler : AMRpcHandler<A0001_Register_C2R, A0001_Register_R2C>
    {
        protected override async ETTask Run(Session session, A0001_Register_C2R request, A0001_Register_R2C response, Action reply)
        {
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                //验证假定的账号和密码
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}'}}");
                if (result.Count == 1)
                {

                    response.Error = ErrorCode.ERR_AccountAlreadyRegisted;
                    reply();
                    return;
                }
                else if (result.Count > 1)
                {
                    response.Error = ErrorCode.ERR_RepeatedAccountExist;
                    Log.Error("出现重复账号：" + request.Account);
                    reply();
                    return;
                }

                //生成玩家帐号 这里随机生成区号
                AccountInfo newAccount = ComponentFactory.CreateWithId<AccountInfo>(RealmHelper.GenerateId());
                newAccount.Account = request.Account;
                newAccount.Password = request.Password;
                await dbProxy.Save(newAccount);

                //生成玩家的用户信息 用户名在消息中提供
                UserInfo newUser = ComponentFactory.CreateWithId<UserInfo, string>(newAccount.Id, request.Account);
                await dbProxy.Save(newUser);

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