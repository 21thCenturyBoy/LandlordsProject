using ETModel;
using System.Net;

namespace ETHotfix
{
    [ObjectSystem]
    public class SessionUserComponentDestroySystem : DestroySystem<SessionUserComponent>
    {
        public override void Destroy(SessionUserComponent self)
        {
            try
            {
                //释放User对象时将User对象从管理组件中移除
                Log.Info($"销毁User和Session{self.User.UserID}");
                Game.Scene.GetComponent<UserComponent>().Remove(self.User.UserID);

                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();

                //向登录服务器发送玩家下线消息
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
                realmSession.Send(new A0005_PlayerOffline_G2R() { UserID = self.User.UserID });

                //服务端主动断开客户端连接
                Game.Scene.GetComponent<NetOuterComponent>().Remove(self.User.GateSessionID);
                //Log.Info($"将玩家{message.UserID}连接断开");

                self.User.Dispose();
                self.User = null;
            }
            catch (System.Exception e)
            {
                Log.Trace(e.ToString());
            }
        }
    }
}