namespace ETModel
{
    [Event(ETModel.EventIdType.SelectHandCard)]
    public class SelectHandCardEvent : AEvent<Card>
    {
        public override void Run(Card card)
        {
            Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom).GetComponent<LandRoomComponent>().Interaction.SelectCard(card);
        }
    }
}