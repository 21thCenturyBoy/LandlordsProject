using System;
using System.Collections.Generic;

namespace ETModel
{

    public static partial class LandUIType
    {
        //登录
        public const string LandLogin = "LandLogin";
        //大厅
        public const string LandLobby = "LandLobby";
        public const string SetUserInfo = "SetUserInfo";
        //房间
        public const string LandRoom = "LandRoom";
        //提示
        public const string LandTip = "LandTip";
    }

    public static partial class UIEventType
    {
        //斗地主EventIdType
        //登录
        public const string LandInitSceneStart = "LandInitSceneStart";
        public const string LandLoginFinish = "LandLoginFinish";
        //大厅
        public const string LandInitLobby = "LandInitLobby";
        public const string LandInitSetUserInfo = "LandInitSetUserInfo";
        public const string LandSetUserInfoFinish = "LandSetUserInfoFinish";
        //断开链接
        public const string LandOpenTip = "LandOpenTip";
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
    //初始设置用户信息事件方法
    [Event(UIEventType.LandInitSetUserInfo)]
    public class LandInitUserInfo_CreateSetUserInfo : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.SetUserInfo);
        }
    }
    //登录完成移除登录界面事件方法
    [Event(UIEventType.LandSetUserInfoFinish)]
    public class LandSetUserInfoFinish : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.SetUserInfo);

        }
    }

    //登录完成移除登录界面事件方法
    [Event(UIEventType.LandOpenTip)]
    public class LandOpenTip : AEvent<string>
    {
        public override void Run(string info)
        {
            UI tip = Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandTip);
            tip.GetComponent<LandTipComponent>().prompt.text = info;
        }
    }
}