using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class A0005_PlayerOfflineHandler : AMHandler<A0005_PlayerOffline_G2R>
    {
        protected override async ETTask Run(Session session, A0005_PlayerOffline_G2R message)
        {
            //玩家下线
            Game.Scene.GetComponent<OnlineComponent>().Remove(message.UserID);
            Log.Info($"玩家{message.UserID}下线");
        }
    }
}