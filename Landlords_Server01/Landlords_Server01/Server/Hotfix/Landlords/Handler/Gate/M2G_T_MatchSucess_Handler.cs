using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class M2G_T_MatchSucess_Handler : AMHandler<MatchSucess_M2G>
    {
        protected override async ETTask Run(Session session, MatchSucess_M2G message)
        {
            User user = Game.Scene.GetComponent<UserComponent>().Get(message.UserID);
            //gate更新ActorID
            user.ActorID = message.GamerID;
            Log.Info($"玩家{user.UserID}匹配成功 更新客户端Actor转发向Gamer2");

            await ETTask.CompletedTask;
        }
    }
}