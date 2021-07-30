using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_TestMessageHandler : AMRpcHandler<C2G_TestMessage, G2C_TestMessage>
	{
		protected override async ETTask Run(Session session, C2G_TestMessage request, G2C_TestMessage response, Action reply)
		{
			response.Message = "前端的朋友，消息收到了<<===";
			reply();
		}
	}
}