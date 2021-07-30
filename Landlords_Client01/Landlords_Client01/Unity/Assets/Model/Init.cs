using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        private void Start()
        {
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                //客户端配置
                ClientConfigHelper.SetConfigHelper();
                //加载程序集
                Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                //添加组件
                Game.Scene.AddComponent<GlobalConfigComponent>();//web资源服务器设置组件
                Game.Scene.AddComponent<ResourcesComponent>();//资源加载组件

                //测试输出正确加载Config所带的信息
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

                UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
                Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

                #region Example

                ////练习1
                //Game.Scene.AddComponent<OpcodeTestComponent>();
                ////练习2
                //Game.Scene.AddComponent<TimerComponent>();
                //Game.Scene.AddComponent<FrameTestComponent>();

                ////练习3
                //TestRoom room = ComponentFactory.Create<TestRoom>();
                //room.AddComponent<TimeTestComponent>();
                //room.GetComponent<TimeTestComponent>().Run(Typebehavior.Waiting, 5000);
                //room.GetComponent<TimeTestComponent>().Run(Typebehavior.RandTarget);


                //测试发送给服务端一条文本消息
                //Session session = Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                //G2C_TestMessage g2CTestMessage = (G2C_TestMessage) await session.Call(new C2G_TestMessage() { Info = "==>>服务端的朋友,你好!收到请回答" });

                ////下载AB包
                //await BundleHelper.DownloadBundle();
                //Game.EventSystem.Load();

                #endregion

                //添加UI组件
                Game.Scene.AddComponent<UIComponent>();

                Game.Scene.AddComponent<GamerComponent>();
                //加上消息分发组件MessageDispatcherComponent
                Game.Scene.AddComponent<MessageDispatcherComponent>();


                //执行斗地主初始事件，也就是创建LandLogin界面
                Game.EventSystem.Run(UIEventType.LandInitSceneStart);

                //添加指令与网络组件
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();




            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.EventSystem.Update();
            
        }

        private void LateUpdate()
        {
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}