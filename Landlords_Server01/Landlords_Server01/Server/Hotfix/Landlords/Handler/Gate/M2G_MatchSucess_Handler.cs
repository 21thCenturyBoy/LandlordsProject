using ETModel;

[ActorMessageHandler(AppType.Gate)]
public class M2G_MatchSucess_Handler : AMActorHandler<User, Actor_MatchSucess_M2G>
{
    protected override async ETTask Run(User user, Actor_MatchSucess_M2G message)
    {
        //gate更新ActorID
        user.ActorID = message.GamerID;
        Log.Info($"玩家{user.UserID}匹配成功 更新客户端Actor转发向Gamer");
        await ETTask.CompletedTask;
    }
}