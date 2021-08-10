using ETModel;
namespace ETModel
{
//牌类消息
	[Message(OuterOpcode.Card)]
	public partial class Card {}

//牌分值消息
	[Message(OuterOpcode.GamerCardNum)]
	public partial class GamerCardNum {}

//游戏开始玩家手牌消息
	[Message(OuterOpcode.Actor_GameStartHandCards_Ntt)]
	public partial class Actor_GameStartHandCards_Ntt : IActorMessage {}

//获取房间内玩家信息请求
	[Message(OuterOpcode.C2G_GetUserInfoInRoom_Req)]
	public partial class C2G_GetUserInfoInRoom_Req : IRequest {}

//获取房间内玩家信息返回
	[Message(OuterOpcode.G2C_GetUserInfoInRoom_Back)]
	public partial class G2C_GetUserInfoInRoom_Back : IResponse {}

//----ET
	[Message(OuterOpcode.Actor_Test)]
	public partial class Actor_Test : IActorMessage {}

	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

}
namespace ETModel
{
	public static partial class OuterOpcode
	{
		 public const ushort Card = 101;
		 public const ushort GamerCardNum = 102;
		 public const ushort Actor_GameStartHandCards_Ntt = 103;
		 public const ushort C2G_GetUserInfoInRoom_Req = 104;
		 public const ushort G2C_GetUserInfoInRoom_Back = 105;
		 public const ushort Actor_Test = 106;
		 public const ushort C2M_TestRequest = 107;
		 public const ushort M2C_TestResponse = 108;
		 public const ushort Actor_TransferRequest = 109;
		 public const ushort Actor_TransferResponse = 110;
		 public const ushort C2G_EnterMap = 111;
		 public const ushort G2C_EnterMap = 112;
		 public const ushort UnitInfo = 113;
		 public const ushort M2C_CreateUnits = 114;
		 public const ushort Frame_ClickMap = 115;
		 public const ushort M2C_PathfindingResult = 116;
		 public const ushort C2R_Ping = 117;
		 public const ushort R2C_Ping = 118;
		 public const ushort G2C_Test = 119;
		 public const ushort C2M_Reload = 120;
		 public const ushort M2C_Reload = 121;
	}
}
