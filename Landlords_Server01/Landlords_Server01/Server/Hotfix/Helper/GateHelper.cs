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
    }
}