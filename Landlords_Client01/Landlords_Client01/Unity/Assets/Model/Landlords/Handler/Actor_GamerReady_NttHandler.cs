using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerReady_NttHandler : AMHandler<Actor_GamerReady_Landlords>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerReady_Landlords message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Gamer gamer = room.GetGamer(message.UserID);
            gamer.GetComponent<LandlordsGamerPanelComponent>().SetReady();

            //本地玩家准备,隐藏准备按钮
            if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
            {
                uiRoom.GameObject.Get<GameObject>("Ready").SetActive(false);
            }

            await ETTask.CompletedTask;
        }
    }
}