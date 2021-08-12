namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerPlayCard_NttHandler : AMHandler<Actor_GamerPlayCard_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerPlayCard_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent room = uiRoom.GetComponent<LandRoomComponent>();
            Gamer gamer = room.GetGamer(message.UserID);
            if (gamer != null)
            {
                gamer.GetComponent<LandlordsGamerPanelComponent>().ResetPrompt();

                //本地玩家清空选中牌 关闭出牌按钮
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    LandInteractionComponent interaction = uiRoom.GetComponent<LandRoomComponent>().Interaction;
                    interaction.Clear();
                    interaction.EndPlay();
                }

                //出牌后更新玩家手牌
                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                Card[] Tcards = new Card[message.Cards.Count];
                for (int i = 0; i < message.Cards.Count; i++)
                {
                    Tcards[i] = message.Cards[i];
                }
                handCards.PopCards(Tcards);
            }

            await ETTask.CompletedTask;
        }
    }
}