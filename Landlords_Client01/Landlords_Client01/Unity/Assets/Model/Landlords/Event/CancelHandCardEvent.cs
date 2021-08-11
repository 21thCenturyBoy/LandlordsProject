namespace ETModel
{
    [Event(ETModel.EventIdType.CancelHandCard)]
    public class CancelHandCardEvent : AEvent<Card>
    {
        public override void Run(Card card)
        {
            Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom).GetComponent<LandRoomComponent>().Interaction.CancelCard(card);
        }
    }
}