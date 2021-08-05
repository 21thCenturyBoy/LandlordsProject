using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class LandTipComponentAwakeSystem : AwakeSystem<LandTipComponent>
    {
        public override void Awake(LandTipComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 修改设置用户信息界面组件
    /// </summary>
    public class LandTipComponent : Component
    {

        public Text prompt;
        //邮箱
        public Button sureBtn;
        //性别
        public InputField sex;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            sureBtn = rc.Get<GameObject>("SureBtn").GetComponent<Button>();

            sureBtn.onClick.Add(CloseTip);

        }

        public async void CloseTip()
        {
            try
            {
                Game.Scene.GetComponent<UIComponent>().Remove(LandUIType.LandTip);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}