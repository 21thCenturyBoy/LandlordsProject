using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_AuthorityGrabLandlord_NttHandler : AMHandler<Actor_AuthorityGrabLandlord_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_AuthorityGrabLandlord_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);

            if (message.UserID == LandRoomComponent.LocalGamer.UserID)
            {
                //显示抢地主交互
                uiRoom.GetComponent<LandRoomComponent>().Interaction.StartGrab();
            }

            await ETTask.CompletedTask;
        }
    }
}