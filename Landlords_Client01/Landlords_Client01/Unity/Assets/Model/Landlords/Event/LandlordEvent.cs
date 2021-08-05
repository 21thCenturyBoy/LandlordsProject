
namespace ETModel
{
    // 分发数值监听
    [Event(EventIdType.SessionDispose)]
    public class SessionDisposeEvent : AEvent<int>
    {
        public override void Run(int code)
        {
            Game.Scene.GetComponent<UIComponent>().RemoveAll();
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLogin);
            Game.EventSystem.Run(UIEventType.LandOpenTip,$"与服务器已断开，请重新登录...\n错误代码：{code}");
        }
    }
}

