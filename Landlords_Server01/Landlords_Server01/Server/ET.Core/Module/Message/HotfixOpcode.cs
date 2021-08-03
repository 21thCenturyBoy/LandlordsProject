using ETModel;
namespace ETModel
{
//测试向服务器发送消息
	[Message(HotfixOpcode.C2G_TestMessage)]
	public partial class C2G_TestMessage : IRequest {}

//测试向服务器返回消息
	[Message(HotfixOpcode.G2C_TestMessage)]
	public partial class G2C_TestMessage : IResponse {}

//获取用户信息
	[Message(HotfixOpcode.A1001_GetUserInfo_C2G)]
	public partial class A1001_GetUserInfo_C2G : IRequest {}

//返回用户信息
	[Message(HotfixOpcode.A1001_GetUserInfo_G2C)]
	public partial class A1001_GetUserInfo_G2C : IResponse {}

//设置用户信息
	[Message(HotfixOpcode.A1002_SetUserInfo_C2G)]
	public partial class A1002_SetUserInfo_C2G : IRequest {}

//返回设置用户信息
	[Message(HotfixOpcode.A1002_SetUserInfo_G2C)]
	public partial class A1002_SetUserInfo_G2C : IResponse {}

//客户端登陆网关请求
	[Message(HotfixOpcode.A0003_LoginGate_C2G)]
	public partial class A0003_LoginGate_C2G : IRequest {}

//客户端登陆网关返回
	[Message(HotfixOpcode.A0003_LoginGate_G2C)]
	public partial class A0003_LoginGate_G2C : IResponse {}

//客户端登陆认证请求
	[Message(HotfixOpcode.A0002_Login_C2R)]
	public partial class A0002_Login_C2R : IRequest {}

//客户端登陆认证返回
	[Message(HotfixOpcode.A0002_Login_R2C)]
	public partial class A0002_Login_R2C : IResponse {}

//客户端注册请求
	[Message(HotfixOpcode.A0001_Register_C2R)]
	public partial class A0001_Register_C2R : IRequest {}

//客户端注册请求回复
	[Message(HotfixOpcode.A0001_Register_R2C)]
	public partial class A0001_Register_R2C : IResponse {}

//ET----
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate_Req)]
	public partial class C2G_LoginGate_Req : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate_Back)]
	public partial class G2C_LoginGate_Back : IResponse {}

}
namespace ETModel
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2G_TestMessage = 10001;
		 public const ushort G2C_TestMessage = 10002;
		 public const ushort A1001_GetUserInfo_C2G = 10003;
		 public const ushort A1001_GetUserInfo_G2C = 10004;
		 public const ushort A1002_SetUserInfo_C2G = 10005;
		 public const ushort A1002_SetUserInfo_G2C = 10006;
		 public const ushort A0003_LoginGate_C2G = 10007;
		 public const ushort A0003_LoginGate_G2C = 10008;
		 public const ushort A0002_Login_C2R = 10009;
		 public const ushort A0002_Login_R2C = 10010;
		 public const ushort A0001_Register_C2R = 10011;
		 public const ushort A0001_Register_R2C = 10012;
		 public const ushort C2R_Login = 10013;
		 public const ushort R2C_Login = 10014;
		 public const ushort C2G_LoginGate = 10015;
		 public const ushort G2C_LoginGate = 10016;
		 public const ushort G2C_TestHotfixMessage = 10017;
		 public const ushort C2M_TestActorRequest = 10018;
		 public const ushort M2C_TestActorResponse = 10019;
		 public const ushort PlayerInfo = 10020;
		 public const ushort C2G_PlayerInfo = 10021;
		 public const ushort G2C_PlayerInfo = 10022;
		 public const ushort C2G_LoginGate_Req = 10023;
		 public const ushort G2C_LoginGate_Back = 10024;
	}
}
