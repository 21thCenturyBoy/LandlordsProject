using System;
using UnityEngine;

namespace ETModel
{
    [UIFactory(LandUIType.LandLogin)]
    public class LandLoginFactory : IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                //加载AB包
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");

                //加载登录注册界面预设并生成实例
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject landLogin = UnityEngine.Object.Instantiate(bundleGameObject);

                //设置UI层级，只有UI摄像机可以渲染
                landLogin.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(landLogin);

                ui.AddComponent<LandLoginComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
    }
}