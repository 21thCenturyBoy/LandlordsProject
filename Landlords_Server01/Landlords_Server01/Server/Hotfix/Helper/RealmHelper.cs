using ETModel;
using System.Net;
using System.Threading.Tasks;

namespace ETHotfix
{
    public static class RealmHelper
    {
        private static int value;
        /// <summary>
        /// 随机生成区号 1~
        /// </summary>
        public static long GenerateId()
        {
            //随机获得GateId 1~2
            int randomGateAppId = RandomHelper.RandomNumber(0, StartConfigComponent.Instance.GateConfigs.Count) + 1;
            long time = TimeHelper.ClientNowSeconds();
            //1540 2822 75   时间为10位数
            //区号取第11位数
            return (randomGateAppId * 100000000000 + time + ++value);
        }

        /// <summary>
        /// 生成指定大区的账号 参数为1以上的整数
        /// </summary>
        /// <param name="GateAppId"></param>
        /// <returns></returns>
        public static long GenerateId(int GateAppId)
        {
            long time = TimeHelper.ClientNowSeconds();
            //1540 2822 75   时间为10位数
            //区号取第11位数
            return (GateAppId * 100000000000 + time + ++value);
        }

        /// <summary>
        /// 查询账号所在大区 参数为1以上的整数
        /// </summary>
        public static int GetGateAppIdFromUserId(long userID)
        {
            return (int)(userID / 100000000000);
        }
    }
}