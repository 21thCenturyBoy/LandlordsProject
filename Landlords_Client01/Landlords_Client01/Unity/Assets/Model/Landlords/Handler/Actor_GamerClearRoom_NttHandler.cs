using ETModel;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerClearRoom_NttHandler : AMHandler<Actor_GamerClearRoom_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerClearRoom_Ntt message)
        {
            //切换到大厅界面
            if (Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandEnd) != null)
            {
                Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandEnd);
            }
            //切换到大厅界面
            if (Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandInteraction) != null)
            {
                Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandInteraction);
            }
            Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandRoom);
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLobby);
            UI tip = Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandTip);
            tip.GetComponent<LandTipComponent>().prompt.text = "清理房间...\n返回大厅";

            await ETTask.CompletedTask;
        }
    }
}