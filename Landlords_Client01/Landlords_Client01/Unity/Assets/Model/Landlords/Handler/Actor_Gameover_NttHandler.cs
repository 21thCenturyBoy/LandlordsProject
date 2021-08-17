using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [MessageHandler]
    public class Actor_Gameover_NttHandler : AMHandler<Actor_Gameover_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_Gameover_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            ReferenceCollector rc = uiRoom.GameObject.GetComponent<ReferenceCollector>();
            //隐藏发牌桌
            rc.Get<GameObject>("Desk").SetActive(false);

            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Identity localGamerIdentity = LandRoomComponent.LocalGamer.GetComponent<HandCardsComponent>().AccessIdentity;
            UI uiEndPanel = LandEndFactory.Create(LandUIType.LandEnd, uiRoom, message.Winner == (int)localGamerIdentity);
            LandEndComponent landlordsEndComponent = uiEndPanel.GetComponent<LandEndComponent>();

            foreach (GamerScore gamerScore in message.GamersScore)
            {
                Gamer gamer = room.GetGamer(gamerScore.UserID);
                //更新玩家信息（金钱/头像）
                gamer.GetComponent<LandlordsGamerPanelComponent>().UpdatePanel();
                //清空出牌
                gamer.GetComponent<HandCardsComponent>().ClearPlayCards();
                gamer.GetComponent<HandCardsComponent>().Hide();
                //根据GamersScore中玩家顺序进行排列
                landlordsEndComponent.CreateGamerContent(
                    gamer,
                    localGamerIdentity,
                    message.BasePointPerMatch,
                    message.Multiples,
                    gamerScore.Score);
            }

            room.Interaction.Gameover();
            room.ResetMultiples();
            await ETTask.CompletedTask;
        }
    }
}