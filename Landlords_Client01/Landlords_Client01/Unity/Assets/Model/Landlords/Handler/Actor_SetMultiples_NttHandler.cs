using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_SetMultiples_NttHandler : AMHandler<Actor_SetMultiples_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_SetMultiples_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            uiRoom.GetComponent<LandRoomComponent>().SetMultiples(message.Multiples);

            await ETTask.CompletedTask;
        }
    }
}