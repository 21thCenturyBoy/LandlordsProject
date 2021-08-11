using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_AuthorityPlayCard_NttHandler : AMHandler<Actor_AuthorityPlayCard_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_AuthorityPlayCard_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Gamer gamer = room.GetGamer(message.UserID);
            if (gamer != null)
            {
                //重置玩家提示
                gamer.GetComponent<LandlordsGamerPanelComponent>().ResetPrompt();

                //当玩家为先手，清空出牌
                if (message.IsFirst)
                {
                    gamer.GetComponent<HandCardsComponent>().ClearPlayCards();
                }

                //显示出牌按钮
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    LandInteractionComponent interaction = uiRoom.GetComponent<LandRoomComponent>().Interaction;
                    interaction.IsFirst = message.IsFirst;
                    interaction.StartPlay();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}