using System;
using System.Collections.Generic;

namespace ETModel
{

    public static partial class LandUIType
    {
        public const string LandLogin = "LandLogin";
    }

    public static partial class UIEventType
    {
        public const string LandInitSceneStart = "LandInitSceneStart";
        public const string LandLoginFinish = "LandLoginFinish";
    }

    [Event(UIEventType.LandInitSceneStart)]
    public class InitSceneStart_CreateLandLogin : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLogin);
        }
    }
    //移除登录界面事件
    [Event(UIEventType.LandLoginFinish)]
    public class LandLoginFinish : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandLogin);
        }
    }
}