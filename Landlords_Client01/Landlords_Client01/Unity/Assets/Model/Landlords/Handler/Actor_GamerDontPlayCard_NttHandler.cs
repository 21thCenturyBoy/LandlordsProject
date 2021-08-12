using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerDontPlayCard_NttHandler : AMHandler<Actor_GamerDontPlayCard_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerDontPlayCard_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Gamer gamer = room.GetGamer(message.UserID);
            if (gamer != null)
            {
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    uiRoom.GetComponent<LandRoomComponent>().Interaction.EndPlay();
                }
                gamer.GetComponent<HandCardsComponent>().ClearPlayCards();
                gamer.GetComponent<LandlordsGamerPanelComponent>().SetDiscard();
            }

            await ETTask.CompletedTask;
        }
    }
}