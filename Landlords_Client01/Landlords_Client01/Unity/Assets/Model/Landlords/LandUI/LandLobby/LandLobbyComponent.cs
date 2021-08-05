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
        //电话
        public Text phone;
        //邮箱
        public Text email;
        //性别
        public Text sex;

        public bool isMatching;

        public async void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            name = rc.Get<GameObject>("Name").GetComponent<Text>();
            money = rc.Get<GameObject>("Money").GetComponent<Text>();
            level = rc.Get<GameObject>("Level").GetComponent<Text>();
            title = rc.Get<GameObject>("Title").GetComponent<Text>();

            phone = rc.Get<GameObject>("Phone").GetComponent<Text>();
            email = rc.Get<GameObject>("Email").GetComponent<Text>();
            sex = rc.Get<GameObject>("Sex").GetComponent<Text>();

            rc.Get<GameObject>("SetUserInfoBtn").GetComponent<Button>().onClick.Add(OnSetUserInfo);

            //获取玩家数据
            A1001_GetUserInfo_C2G GetUserInfo_Req = new A1001_GetUserInfo_C2G();
            A1001_GetUserInfo_G2C GetUserInfo_Ack = (A1001_GetUserInfo_G2C)await SessionComponent.Instance.Session.Call(GetUserInfo_Req);

            //显示用户名和用户等级
            name.text = GetUserInfo_Ack.UserName;
            money.text = GetUserInfo_Ack.Money.ToString();
            level.text = GetUserInfo_Ack.Level.ToString();
            title.text = GetUserInfo_Ack.Title.ToString();
            email.text = GetUserInfo_Ack.Email.ToString();
            sex.text = GetUserInfo_Ack.Sex.ToString();
            phone.text = GetUserInfo_Ack.Phone.ToString();

            //匹配进入房间按钮
            rc.Get<GameObject>("EnterBtn").GetComponent<Button>().onClick.Add(OnStartMatchLandlords);

            //添加新的匹配目标
            //...

        }
        public void OnSetUserInfo()
        {
            //加载设置用户信息界面
            Game.EventSystem.Run(UIEventType.LandInitSetUserInfo);
        }

        public void UpdateUserInfo(A1002_SetUserInfo_G2C info)
        {
            phone.text = info.Phone.ToString();
            email.text = info.Email;
            sex.text = info.Sex;
        }
        /// <summary>
        /// 匹配斗地主
        /// </summary>
        public async void OnStartMatchLandlords()
        {
            try
            {
                //发送开始匹配消息
                C2G_StartMatch_Req c2G_StartMatch_Req = new C2G_StartMatch_Req();
                G2C_StartMatch_Back g2C_StartMatch_Ack = (G2C_StartMatch_Back)await SessionComponent.Instance.Session.Call(c2G_StartMatch_Req);

                if (g2C_StartMatch_Ack.Error == ErrorCode.ERR_UserMoneyLessError)
                {
                    Log.Error("余额不足");
                    return;
                }

                //切换到房间界面
                UI landRoom = Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandRoom);
                Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandLobby);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}