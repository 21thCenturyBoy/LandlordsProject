using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    /// <summary>
    /// 玩家UI组件
    /// </summary>
    public class LandlordsGamerPanelComponent : Component
    {
        //UI面板
        public GameObject Panel;

        //玩家昵称
        public string NickName { get { return name.text; } }

        private Image headPhoto;
        private Text prompt;
        private Text name;
        private Text money;

        /// <summary>
        /// 设置面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(GameObject panel)
        {
            this.Panel = panel;

            //绑定关联
            this.prompt = this.Panel.Get<GameObject>("Prompt").GetComponent<Text>();
            this.name = this.Panel.Get<GameObject>("Name").GetComponent<Text>();
            this.money = this.Panel.Get<GameObject>("Money").GetComponent<Text>();
            this.headPhoto = this.Panel.Get<GameObject>("HeadPhoto").GetComponent<Image>();

            UpdatePanel();
        }

        /// <summary>
        /// 更新面板
        /// </summary>
        public void UpdatePanel()
        {
            if (this.Panel != null)
            {
                SetUserInfoInRoom();
                //没抢地主前都显示农民头像
                headPhoto.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 玩家准备
        /// </summary>
        public void SetReady()
        {
            prompt.text = "准备！";
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        public void GameStart()
        {
            ResetPrompt();
        }

        /// <summary>
        /// 重置提示
        /// </summary>
        public void ResetPrompt()
        {
            prompt.text = "";
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="id"></param>
        private async void SetUserInfoInRoom()
        {
            G2C_GetUserInfoInRoom_Back g2C_GetUserInfo_Ack = (G2C_GetUserInfoInRoom_Back)await SessionComponent.Instance.Session.Call(new C2G_GetUserInfoInRoom_Req() { UserID = this.GetParent<Gamer>().UserID });

            if (this.Panel != null)
            {
                name.text = g2C_GetUserInfo_Ack.NickName;
                money.text = g2C_GetUserInfo_Ack.Money.ToString();
            }
        }

        /// <summary>
        /// 重置面板
        /// </summary>
        public void ResetPanel()
        {
            ResetPrompt();
            this.headPhoto.gameObject.SetActive(false);
            this.name.text = "空位";
            this.money.text = "";

            this.Panel = null;
            this.prompt = null;
            this.name = null;
            this.money = null;
            this.headPhoto = null;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //重置玩家UI
            ResetPanel();
        }
    }
}