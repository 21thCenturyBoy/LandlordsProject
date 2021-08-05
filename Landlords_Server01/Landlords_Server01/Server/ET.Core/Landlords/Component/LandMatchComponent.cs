using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 斗地主匹配组件，匹配逻辑在LandordsComponentSystem扩展
    /// </summary>
    public class LandMatchComponent : Component
    {
        /// <summary>
        /// 所有游戏中的房间列表
        /// </summary>
        public readonly Dictionary<long, Room> GamingLandlordsRooms = new Dictionary<long, Room>();

        /// <summary>
        /// 所有游戏没有开始的房间列表 Room.Id/Room
        /// </summary>
        public readonly Dictionary<long, Room> FreeLandlordsRooms = new Dictionary<long, Room>();

        /// <summary>
        /// 所有在房间中待机的玩家 UserID/Room
        /// </summary>
        public readonly Dictionary<long, Room> Waiting = new Dictionary<long, Room>();

        /// <summary>
        /// 所有正在游戏的玩家 UserID/Room
        /// </summary>
        public readonly Dictionary<long, Room> Playing = new Dictionary<long, Room>();

        /// <summary>
        /// 匹配中的玩家队列
        /// </summary>
        public readonly Queue<Gamer> MatchingQueue = new Queue<Gamer>();
    }
}