using ETModel;
using System.Net;

namespace ETHotfix
{
    public static class GateHelper
    {
        /// <summary>
        /// 验证Session是否绑定了玩家
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool SignSession(Session session)
        {
            SessionUserComponent sessionUser = session.GetComponent<SessionUserComponent>();
            if (sessionUser == null || Game.Scene.GetComponent<UserComponent>().Get(sessionUser.User.UserID) == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取斗地主房间配置 不满足要求不能进入房间
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static RoomConfig GetLandlordsConfig(RoomLevel level)
        {
            RoomConfig config = new RoomConfig();
            switch (level)
            {
                case RoomLevel.Lv100:
                    config.BasePointPerMatch = 100;
                    config.Multiples = 1;
                    config.MinThreshold = 100 * 10;
                    break;
            }

            return config;
        }

        /// <summary>
        /// 获取斗地主游戏专用Map服务器的Session
        /// </summary>
        /// <returns></returns>
        public static Session GetMapSession()
        {
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint mapIPEndPoint = config.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            Log.Debug(mapIPEndPoint.ToString());
            Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapIPEndPoint);
            return mapSession;
        }
    }
}