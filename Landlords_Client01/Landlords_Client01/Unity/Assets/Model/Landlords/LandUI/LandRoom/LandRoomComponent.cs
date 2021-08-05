using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class LandRoomComponentAwakeSystem : AwakeSystem<LandRoomComponent>
    {
        public override void Awake(LandRoomComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 大厅界面组件
    /// </summary>
    public class LandRoomComponent : Component
    {
        public readonly Dictionary<long, int> seats = new Dictionary<long, int>();
        public bool Matching { get; set; }
        public readonly Gamer[] gamers = new Gamer[3];

        public Text prompt;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            GameObject quitButton = rc.Get<GameObject>("Quit");
            GameObject readyButton = rc.Get<GameObject>("Ready");
            prompt = rc.Get<GameObject>("MatchPrompt").GetComponent<Text>();

            readyButton.SetActive(false); //默认隐藏
            Matching = true; //进入房间后取消匹配状态

            //绑定事件
            quitButton.GetComponent<Button>().onClick.Add(OnQuit);
            readyButton.GetComponent<Button>().onClick.Add(OnReady);

        }

        public void AddGamer(Gamer gamer, int index)
        {
            seats.Add(gamer.UserID, index);
            gamers[index] = gamer;

            prompt.text = $"一位玩家进入房间，房间人数{seats.Count}";
        }

        public void RemoveGamer(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                Gamer gamer = gamers[seatIndex];
                gamers[seatIndex] = null;
                seats.Remove(id);
                gamer.Dispose();
                prompt.text = $"一位玩家离开房间，房间人数{seats.Count}";
            }
        }

        public int GetGamerSeat(long id)
        {
            int seatIndex;
            if (seats.TryGetValue(id, out seatIndex))
            {
                return seatIndex;
            }
            return -1;
        }

        public void OnQuit()
        {
            //发送退出房间消息
            SessionComponent.Instance.Session.Send(new C2G_ReturnLobby_Ntt());

            //切换到大厅界面
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLobby);
            Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandRoom);
        }

        private void OnReady()
        {
            //发送准备
            //SessionComponent.Instance.Session.Send(new Actor_GamerReady_Landlords());
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Matching = false;

            this.seats.Clear();

            for (int i = 0; i < this.gamers.Length; i++)
            {
                if (gamers[i] != null)
                {
                    gamers[i].Dispose();
                    gamers[i] = null;
                }
            }
        }
    }
}