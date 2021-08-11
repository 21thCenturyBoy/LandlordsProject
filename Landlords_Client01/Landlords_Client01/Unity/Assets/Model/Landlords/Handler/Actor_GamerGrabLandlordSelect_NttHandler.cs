using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerGrabLandlordSelect_NttHandler : AMHandler<Actor_GamerGrabLandlordSelect_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerGrabLandlordSelect_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Gamer gamer = room.GetGamer(message.UserID);
            if (gamer != null)
            {
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    uiRoom.GetComponent<LandRoomComponent>().Interaction.EndGrab();
                }
                gamer.GetComponent<LandlordsGamerPanelComponent>().SetGrab(message.IsGrab);
            }

            await ETTask.CompletedTask;
        }
    }
}