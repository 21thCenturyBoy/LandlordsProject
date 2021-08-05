﻿using System;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
			
			try
			{			
				Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
				Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
                StartConfigComponent configComponent = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId);
				StartConfig startConfig = configComponent.StartConfig;
				
				if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

				IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = $"{startConfig.AppType}";
				LogManager.Configuration.Variables["appId"] = $"{startConfig.AppId}";
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType, -8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:0000}";

				Log.Info($"server start........................ {startConfig.AppId} {startConfig.AppType}");

				Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();

				// 根据不同的AppType添加不同的组件
				OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();

				
				// 发送普通actor消息
				Game.Scene.AddComponent<ActorMessageSenderComponent>();
				// 发送location actor消息
				Game.Scene.AddComponent<ActorLocationSenderComponent>();

                //数据库组件
				//PS：如果启动闪退有可能是服务器配置文件没有填数据库配置，请正确填写
				Game.Scene.AddComponent<DBComponent>();
				//这里需要加上DBCacheComponent才能操作MongoDB
				Game.Scene.AddComponent<DBCacheComponent>();
				Game.Scene.AddComponent<DBProxyComponent>();

                Game.Scene.AddComponent<UserComponent>();
                Game.Scene.AddComponent<SessionKeyComponent>();
                Game.Scene.AddComponent<SessionUserComponent>();

				// location server需要的组件
				Game.Scene.AddComponent<LocationComponent>();
				// 访问location server的组件
				Game.Scene.AddComponent<LocationProxyComponent>();
				
				// 这两个组件是处理actor消息使用的
				Game.Scene.AddComponent<MailboxDispatcherComponent>();
				Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
				
				// 内网消息组件
				Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
				// 外网消息组件
				Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);

                //在线管理组件
                Game.Scene.AddComponent<OnlineComponent>();
                //斗地主匹配组件
                Game.Scene.AddComponent<LandMatchComponent>();

				// manager server组件，用来管理其它进程使用
				Game.Scene.AddComponent<AppManagerComponent>();
				Game.Scene.AddComponent<RealmGateAddressComponent>();
				Game.Scene.AddComponent<GateSessionKeyComponent>();
				Game.Scene.AddComponent<PlayerComponent>();
				Game.Scene.AddComponent<UnitComponent>();

				// 配置管理
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.AddComponent<ConsoleComponent>();

				while (true)
				{
					try
					{
						Thread.Sleep(1);
						OneThreadSynchronizationContext.Instance.Update();
						Game.EventSystem.Update();
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
