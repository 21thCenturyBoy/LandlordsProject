using UnityEngine;
using UnityEngine.UI;
namespace ETModel
{
    [ObjectSystem]
    public class LandLoginComponentAwakeSystem : AwakeSystem<LandLoginComponent>
    {
        public override void Awake(LandLoginComponent self)
        {
            self.Awake();
        }
    }

    public class LandLoginComponent : Component
    {
        //提示文本
        public Text prompt;

        public InputField account;
        public InputField password;

        //是否正在登录中（避免登录请求还没响应时连续点击登录）
        public bool isLogining;
        //是否正在注册中（避免登录请求还没响应时连续点击注册）
        public bool isRegistering;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //初始化数据
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            this.isLogining = false;
            this.isRegistering = false;

            //添加事件
            rc.Get<GameObject>("LoginButton").GetComponent<Button>().onClick.Add(() => LoginBtnOnClick());
            rc.Get<GameObject>("RegisterButton").GetComponent<Button>().onClick.Add(() => RegisterBtnOnClick());
        }

        public void LoginBtnOnClick()
        {
            if (this.isLogining || this.IsDisposed)
            {
                return;
            }
            this.isLogining = true;
            LandHelper.Login(this.account.text, this.password.text).Coroutine();
        }

        public void RegisterBtnOnClick()
        {
            if (this.isRegistering || this.IsDisposed)
            {
                return;
            }
            this.isRegistering = true;
            LandHelper.Register(this.account.text, this.password.text).Coroutine();
        }
    }
}