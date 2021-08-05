using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_StartMatch_Handler : AMRpcHandler<C2G_StartMatch_Req, G2C_StartMatch_Back>
    {
        protected override async ETTask Run(Session session, C2G_StartMatch_Req request, G2C_StartMatch_Back response, Action reply)
        {
            try
            {
                Log.Debug("玩家开始匹配");
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_SignError;
                    reply();
                    return;
                }

                User user = session.GetComponent<SessionUserComponent>().User;

                //验证玩家是否符合进入房间要求,默认为100底分局
                RoomConfig roomConfig = GateHelper.GetLandlordsConfig(RoomLevel.Lv100);
                UserInfo userInfo = await Game.Scene.GetComponent<DBProxyComponent>().Query<UserInfo>(user.UserID);
                if (userInfo.Money < roomConfig.MinThreshold)
                {
                    response.Error = ErrorCode.ERR_UserMoneyLessError;
                    reply();
                    return;
                }

                reply();

                //获取斗地主Map服务器的Session
                //通知Map服务器创建地图上的Gamer
                Session mapSession = GateHelper.GetMapSession();
                mapSession.Send(new EnterMatchs_G2M()
                {
                    UserID = user.UserID,
                    GActorID = user.InstanceId,
                    CActorID = user.GateSessionID
                });

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}