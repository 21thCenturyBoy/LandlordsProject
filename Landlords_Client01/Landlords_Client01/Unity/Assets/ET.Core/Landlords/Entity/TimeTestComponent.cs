using System;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class TimeTestComponentAwakeSystem : AwakeSystem<TimeTestComponent>
    {
        public override void Awake(TimeTestComponent self)
        {
            self.Awake();
        }
    }

    public class TimeTestComponent : Component
    {
        private Entity parent;
        private readonly Dictionary<string, ITimeBehavior> Tbehaviors = new Dictionary<string, ITimeBehavior>();

        public void Awake()
        {
            this.parent = this.GetParent<Entity>();
            Log.Debug(parent.ToString());
            this.Load();
        }

        public void Run(string type, long time = 0)
        {
            try
            {
                Tbehaviors[type].Behavior(parent, time);
            }
            catch (Exception e)
            {
                throw new Exception($"{type} Time Behavior 错误: {e}");
            }
        }

        //扫描程序集，获取所有具有[TimeBehavior]特性的对象
        //都添加到Tbehaviors中
        public void Load()
        {
            this.Tbehaviors.Clear();
            List<Type> types = Game.EventSystem.GetTypes(typeof(TimeBehaviorAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(TimeBehaviorAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                TimeBehaviorAttribute attribute = attrs[0] as TimeBehaviorAttribute;
                if (Tbehaviors.ContainsKey(attribute.Type))
                {
                    Log.Debug($"已经存在同类Time Behavior: {attribute.Type}");
                    throw new Exception($"已经存在同类Time Behavior: {attribute.Type}");
                }
                object o = Activator.CreateInstance(type);
                ITimeBehavior behavior = o as ITimeBehavior;
                if (behavior == null)
                {
                    Log.Error($"{o.GetType().FullName} 没有继承 ITimeBehavior");
                    continue;
                }
                this.Tbehaviors.Add(attribute.Type, behavior);
            }
        }

    }
}