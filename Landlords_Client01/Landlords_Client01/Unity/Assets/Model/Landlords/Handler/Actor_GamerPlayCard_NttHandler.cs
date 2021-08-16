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
                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                //清空旧牌缓存
                gamer.GetComponent<LandlordsGamerPanelComponent>().ResetPrompt();
                handCards.ClearPlayCards();

                //本地玩家清空选中牌 关闭出牌按钮
                if (gamer.UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    LandInteractionComponent interaction = uiRoom.GetComponent<LandRoomComponent>().Interaction;
                    interaction.Clear();
                    interaction.EndPlay();
                }

                //出牌后更新玩家手牌
                Card[] Tcards = new Card[message.Cards.Count];
                for (int i = 0; i < message.Cards.Count; i++)
                {
                    Tcards[i] = Card.Create(message.Cards[i].CardWeight, message.Cards[i].CardSuits);
                }
                handCards.PopCards(Tcards);
            }

            await ETTask.CompletedTask;
        }
    }
}