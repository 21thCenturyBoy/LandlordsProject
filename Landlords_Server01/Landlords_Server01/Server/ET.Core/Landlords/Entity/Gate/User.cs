namespace ETModel
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User, long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }

    /// <summary>
    /// 玩家对象 在C2G_LoginGate_Handler中初始化参数
    /// </summary>
    public sealed class User : Entity
    {
        /// <summary>
        /// 读取自数据库中的永久ID
        /// 本程序中出现的UserID字样变量均代表此意
        /// </summary>
        public long UserID { get; private set; }

        /// <summary>
        /// 玩家所在的Gate服务器的AppID
        /// </summary>    
        public int GateAppID { get; set; }

        /// <summary>
        /// 玩家所绑定的Seesion.Id 用于给客户端发送消息
        /// </summary>
        public long GateSessionID { get; set; }

        /// <summary>
        /// 为客户端转发Actor消息到指定目标
        /// 未进入地图时ActorID=0，进入地图后ActorID=GamerID
        /// </summary>
        public long ActorID { get; set; }

        public void Awake(long id)
        {
            this.UserID = id;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.UserID = 0;
            this.GateAppID = 0;
            this.GateSessionID = 0;
            this.ActorID = 0;
        }
    }
}