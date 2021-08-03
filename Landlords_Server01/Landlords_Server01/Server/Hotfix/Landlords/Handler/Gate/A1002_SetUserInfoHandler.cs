using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class A1002_SetUserInfoHandler : AMRpcHandler<A1002_SetUserInfo_C2G, A1002_SetUserInfo_G2C>
    {
        protected override async ETTask Run(Session session, A1002_SetUserInfo_C2G request, A1002_SetUserInfo_G2C response, Action reply)
        {
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_UserNotOnline;
                    reply();
                    return;
                }

                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                //获取User对象
                User user = session.GetComponent<SessionUserComponent>().User;

                //查询获得用户信息数据对象
                UserInfo userInfo = await dbProxy.Query<UserInfo>(user.UserID);
                userInfo.Phone = request.Phone;
                userInfo.Email = request.Email;
                userInfo.Sex = request.Sex;
                await dbProxy.Save(userInfo);

                response.Phone = userInfo.Phone;
                response.Email = userInfo.Email;
                response.Sex = userInfo.Sex;
                response.Title = userInfo.Title;

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