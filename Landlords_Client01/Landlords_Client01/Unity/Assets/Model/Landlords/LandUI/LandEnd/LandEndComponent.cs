using UnityEngine;
using UnityEngine.UI;
using System;

namespace ETModel
{
    [ObjectSystem]
    public class LandEndComponentAwakeSystem : AwakeSystem<LandEndComponent, bool>
    {
        public override void Awake(LandEndComponent self, bool isWin)
        {
            self.Awake(isWin);
        }
    }

    public class LandEndComponent : Component
    {
        //玩家信息面板预设名称
        public const string CONTENT_NAME = "Content";

        private GameObject contentPrefab;  //每个玩家的统计
        private GameObject gamerContent;  //计分面板

        public void Awake(bool isWin)
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{CONTENT_NAME}.unity3d");

            if (isWin)
            {
                rc.Get<GameObject>("Lose").SetActive(false);
            }
            else
            {
                rc.Get<GameObject>("Win").SetActive(false);
            }

            gamerContent = rc.Get<GameObject>("GamerContent");
            Button continueButton = rc.Get<GameObject>("ContinueButton").GetComponent<Button>();
            continueButton.onClick.Add(OnContinue);
            contentPrefab = (GameObject)resourcesComponent.GetAsset($"{CONTENT_NAME}.unity3d", CONTENT_NAME);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent?.UnloadBundle($"{CONTENT_NAME}.unity3d");
            LandEndFactory.Remove(LandUIType.LandEnd);
        }

        /// <summary>
        /// 创建玩家结算信息
        /// </summary>
        /// <returns></returns>
        public GameObject CreateGamerContent(Gamer gamer, Identity winnerIdentity, long baseScore, int multiples, long score)
        {
            //实例化prefab
            GameObject newContent = UnityEngine.Object.Instantiate(contentPrefab);
            newContent.transform.SetParent(gamerContent.transform, false);

            //获取玩家身份/设置头像
            Identity gamerIdentity = gamer.GetComponent<HandCardsComponent>().AccessIdentity;
            Sprite identitySprite = CardHelper.GetCardSprite($"Identity_{Enum.GetName(typeof(Identity), gamerIdentity)}");
            newContent.Get<GameObject>("Identity").GetComponent<Image>().sprite = identitySprite;

            //设置昵称/底分/倍数/积分
            string nickName = gamer.GetComponent<LandlordsGamerPanelComponent>().NickName;
            Text nickNameText = newContent.Get<GameObject>("NickName").GetComponent<Text>();
            Text baseScoreText = newContent.Get<GameObject>("BaseScore").GetComponent<Text>();
            Text multiplesText = newContent.Get<GameObject>("Multiples").GetComponent<Text>();
            Text scoreText = newContent.Get<GameObject>("Score").GetComponent<Text>();
            nickNameText.text = nickName;
            baseScoreText.text = baseScore.ToString();
            multiplesText.text = multiples.ToString();
            scoreText.text = score.ToString();

            //本地玩家的统计结果高亮
            if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
            {
                nickNameText.color = Color.red;
                baseScoreText.color = Color.red;
                multiplesText.color = Color.red;
                scoreText.color = Color.red;
            }

            return newContent;
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        private void OnContinue()
        {
            UI entity = this.GetParent<UI>();
            UI parent = (UI)entity.Parent;
            parent.GameObject.Get<GameObject>("Ready").SetActive(true);
            parent.Remove(entity.Name);

            //发消息到服务器继续游戏
            SessionComponent.Instance.Session.Send(new Actor_GamerContinue_Ntt());
        }
    }
}