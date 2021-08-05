using ETModel;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerExitRoom_NttHandler : AMHandler<Actor_GamerExitRoom_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerExitRoom_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent landlordsRoomComponent = uiRoom.GetComponent<LandRoomComponent>();
            landlordsRoomComponent.RemoveGamer(message.UserID);

            await ETTask.CompletedTask;
        }
    }
}