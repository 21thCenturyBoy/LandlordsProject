using System;
using System.Collections.Generic;

namespace ETModel
{

    public static partial class LandUIType
    {
        public const string LandLogin = "LandLogin";
        public const string LandLobby = "LandLobby";
    }

    public static partial class UIEventType
    {
        //斗地主EventIdType
        public const string LandInitSceneStart = "LandInitSceneStart";
        public const string LandLoginFinish = "LandLoginFinish";
        public const string LandInitLobby = "LandInitLobby";

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
    //初始化大厅界面事件方法
    [Event(UIEventType.LandInitLobby)]
    public class LandInitLobby_CreateLandLobby : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLobby);
        }
    }
}