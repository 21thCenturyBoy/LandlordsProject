using MongoDB.Bson.Serialization.Attributes;
namespace ETModel
{
    [ObjectSystem]
    public class GamerAwakeSystem : AwakeSystem<Gamer, long>
    {
        public override void Awake(Gamer self, long userid)
        {
            self.Awake(userid);
        }
    }

    /// <summary>
    /// 房间玩家对象
    /// </summary>
    public sealed class Gamer : Entity
    {
        /// <summary>
        /// 来自数据库中的永久ID
        /// </summary>
        public long UserID { get; private set; }

        /// <summary>
        /// 玩家GateActorID
        /// </summary>
        public long GActorID { get; set; }

        /// <summary>
        /// 默认为假 Session断开/离开房间时触发离线
        /// </summary>
        public bool isOffline { get; set; }

        public void Awake(long userid)
        {
            this.UserID = userid;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.UserID = 0;
            this.GActorID = 0;
            this.isOffline = false;
        }
    }
}