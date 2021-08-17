using UnityEngine;
using UnityEngine.UI;
using ETModel;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class LandInteractionComponentAwakeSystem : AwakeSystem<LandInteractionComponent>
    {
        public override void Awake(LandInteractionComponent self)
        {
            self.Awake();
        }
    }
    /// <summary>
    /// 游戏交互组件
    /// </summary>
    public class LandInteractionComponent : Component
    {
        private Button playButton;  //出牌
        private Button promptButton;  //提示
        private Button discardButton;  //不出
        private Button grabButton;   //抢地主
        private Button disgrabButton;  //不抢

        private List<Card> currentSelectCards = new List<Card>();

        public bool IsFirst { get; set; }

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            playButton = rc.Get<GameObject>("PlayButton").GetComponent<Button>();
            promptButton = rc.Get<GameObject>("PromptButton").GetComponent<Button>();
            discardButton = rc.Get<GameObject>("DiscardButton").GetComponent<Button>();
            grabButton = rc.Get<GameObject>("GrabButton").GetComponent<Button>();
            disgrabButton = rc.Get<GameObject>("DisgrabButton").GetComponent<Button>();


            //绑定事件
            playButton.onClick.Add(OnPlay);
            promptButton.onClick.Add(OnPrompt);
            discardButton.onClick.Add(OnDiscard);
            grabButton.onClick.Add(OnGrab);
            disgrabButton.onClick.Add(OnDisgrab);

            //默认隐藏UI
            playButton.gameObject.SetActive(false);
            promptButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
            grabButton.gameObject.SetActive(false);
            disgrabButton.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            Game.Scene.GetComponent<ResourcesComponent>()?.UnloadBundle($"{LandUIType.LandInteraction}.unity3d");
        }


        /// <summary>
        /// 选中卡牌
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(Card card)
        {
            currentSelectCards.Add(card);
        }

        /// <summary>
        /// 取消选中卡牌
        /// </summary>
        /// <param name="card"></param>
        public void CancelCard(Card card)
        {
            currentSelectCards.Remove(card);
        }

        /// <summary>
        /// 清空选中卡牌
        /// </summary>
        public void Clear()
        {
            currentSelectCards.Clear();
        }

        /// <summary>
        /// 开始抢地主
        /// </summary>
        public void StartGrab()
        {
            grabButton.gameObject.SetActive(true);
            disgrabButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// 开始出牌
        /// </summary>
        public void StartPlay()
        {
            playButton.gameObject.SetActive(true);
            promptButton.gameObject.SetActive(!IsFirst);
            discardButton.gameObject.SetActive(!IsFirst);
        }

        /// <summary>
        /// 结束抢地主
        /// </summary>
        public void EndGrab()
        {
            grabButton.gameObject.SetActive(false);
            disgrabButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 结束出牌
        /// </summary>
        public void EndPlay()
        {
            playButton.gameObject.SetActive(false);
            promptButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
        }
        /// <summary>
        /// 游戏结束
        /// </summary>
        public void Gameover()
        {
            promptButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 出牌
        /// </summary>
        private async void OnPlay()
        {
            CardHelper.Sort(currentSelectCards);
            Actor_GamerPlayCard_Req request = new Actor_GamerPlayCard_Req();
            foreach (var a in currentSelectCards)
            {
                request.Cards.Add(a);
            }
            Actor_GamerPlayCard_Back response = (Actor_GamerPlayCard_Back)await SessionComponent.Instance.Session.Call(request);

            //出牌错误提示
            LandlordsGamerPanelComponent gamerUI = LandRoomComponent.LocalGamer.GetComponent<LandlordsGamerPanelComponent>();
            if (response.Error == ErrorCode.ERR_PlayCardError)
            {
                gamerUI.SetPlayCardsError();
            }
        }

        /// <summary>
        /// 提示
        /// </summary>
        private async void OnPrompt()
        {
            Actor_GamerPrompt_Req request = new Actor_GamerPrompt_Req();
            Actor_GamerPrompt_Back response = (Actor_GamerPrompt_Back)await SessionComponent.Instance.Session.Call(request);

            HandCardsComponent handCards = LandRoomComponent.LocalGamer.GetComponent<HandCardsComponent>();

            //清空当前选中
            while (currentSelectCards.Count > 0)
            {
                Card selectCard = currentSelectCards[currentSelectCards.Count - 1];
                handCards.GetSprite(selectCard).GetComponent<HandCardSprite>().OnClick(null);
            }

            //自动选中提示出牌
            if (response.Cards != null)
            {
                for (int i = 0; i < response.Cards.Count; i++)
                {
                    handCards.GetSprite(response.Cards[i]).GetComponent<HandCardSprite>().OnClick(null);
                }
            }
            //没有大过场上的牌
            if (response.Cards.Count==0)
            {
                LandlordsGamerPanelComponent gamerUI = LandRoomComponent.LocalGamer.GetComponent<LandlordsGamerPanelComponent>();
                gamerUI.SetDisCanPlayCards();
            }
        }

        /// <summary>
        /// 不出
        /// </summary>
        private void OnDiscard()
        {
            SessionComponent.Instance.Session.Send(new Actor_GamerDontPlayCard_Ntt());
        }

        /// <summary>
        /// 抢地主
        /// </summary>
        private void OnGrab()
        {
            SessionComponent.Instance.Session.Send(new Actor_GamerGrabLandlordSelect_Ntt() { IsGrab = true });
        }

        /// <summary>
        /// 不抢
        /// </summary>
        private void OnDisgrab()
        {
            SessionComponent.Instance.Session.Send(new Actor_GamerGrabLandlordSelect_Ntt() { IsGrab = false });
        }
    }
}