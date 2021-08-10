using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using System.Net;

namespace ETHotfix
{
    public static class MapHelper
    {
        public static Session GetGateSession()
        {
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint gateIPEndPoint = config.GateConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            //Log.Debug(gateIPEndPoint.ToString());
            Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateIPEndPoint);
            return gateSession;
        }

    }

    /// <summary>
    /// 容器转换辅助方法
    /// </summary>
    public static class To
    {
        //数组-RepeatedField
        public static RepeatedField<T> RepeatedField<T>(T[] cards)
        {
            RepeatedField<T> a = new RepeatedField<T>();
            foreach (var b in cards)
            {
                a.Add(b);
            }
            return a;
        }

        //列表-RepeatedField
        public static RepeatedField<T> RepeatedField<T>(List<T> cards)
        {
            RepeatedField<T> a = new RepeatedField<T>();
            foreach (var b in cards)
            {
                a.Add(b);
            }
            return a;
        }

        //重复字段-RepeatedField
        public static T[] Array<T>(RepeatedField<T> cards)
        {
            T[] a = new T[cards.Count];
            for (int i = 0; i < cards.Count; i++)
            {
                a[i] = cards[i];
            }
            return a;
        }

        //重复字段-列表
        public static List<T> List<T>(RepeatedField<T> cards)
        {
            List<T> a = new List<T>();
            foreach (var b in cards)
            {
                a.Add(b);
            }
            return a;
        }
    }
}