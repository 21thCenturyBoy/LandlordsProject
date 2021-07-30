using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    //给组件加系统Awake事件扩展类和扩展方法
    //添加Awake事件，组件添加到实体时，就会执行组件扩展的Awake方法
    //继承AwakeSystem<T>，以被扩展的组件名作为型参
    [ObjectSystem]
    public class OpcodeTestComponentAwakeSystem : AwakeSystem<OpcodeTestComponent>
    {
        public override void Awake(OpcodeTestComponent self)
        {
            self.Awake();
        }
    }
    //这里我把系统Load事件放在Program中添加的所有组件添加完成，并下载完AB包后调用。（当然也可以自己调整Load生命周期的调用时机）
    [ObjectSystem]
    public class OpcodeTestComponentLoadSystem : LoadSystem<OpcodeTestComponent>
    {
        public override void Load(OpcodeTestComponent self)
        {
            self.Load();
        }
    }
    //测试组件
    public class OpcodeTestComponent : Component
    {
        public void Awake()
        {
            Log.Info("程序集初始，执行- OpcodeTest- Awake方法");
        }
        public void Load()
        {
            Log.Info("加载完成，执行- OpcodeTest- Load方法");
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}

