using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class LandLobbyComponentAwakeSystem : AwakeSystem<LandLobbyComponent>
    {
        public override void Awake(LandLobbyComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 大厅界面组件
    /// </summary>
    public class LandLobbyComponent : Component
    {
        //提示文本
        public Text prompt;
        //玩家名称
        private Text name;
        //玩家金钱
        private Text money;
        //玩家等级
        private Text level;
        //玩家称号
        private Text title;

        public bool isMatching;

        public async void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            name = rc.Get<GameObject>("Name").GetComponent<Text>();
            money = rc.Get<GameObject>("Money").GetComponent<Text>();
            level = rc.Get<GameObject>("Level").GetComponent<Text>();
            title = rc.Get<GameObject>("Title").GetComponent<Text>();

            //获取玩家数据
            A1001_GetUserInfo_C2G GetUserInfo_Req = new A1001_GetUserInfo_C2G();
            A1001_GetUserInfo_G2C GetUserInfo_Ack = (A1001_GetUserInfo_G2C)await SessionComponent.Instance.Session.Call(GetUserInfo_Req);

            //显示用户名和用户等级
            name.text = GetUserInfo_Ack.UserName;
            money.text = GetUserInfo_Ack.Money.ToString();
            level.text = GetUserInfo_Ack.Level.ToString();
            title.text = GetUserInfo_Ack.Title.ToString();
            Debug.Log(GetUserInfo_Ack.Title);
            //添加进入房间匹配事件
            //...

            //添加新的匹配目标
            //...

        }
    }
}