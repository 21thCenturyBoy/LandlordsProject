using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerEnterRoom_NttHandler : AMHandler<Actor_GamerEnterRoom_Ntt>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_GamerEnterRoom_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandRoom);
            LandRoomComponent landRoomComponent = uiRoom.GetComponent<LandRoomComponent>();

            //从匹配状态中切换为准备状态
            if (landRoomComponent.Matching)
            {
                landRoomComponent.Matching = false; //进入房间取消匹配状态
                GameObject matchPrompt = uiRoom.GameObject.Get<GameObject>("MatchPrompt");

                uiRoom.GameObject.Get<GameObject>("Ready").SetActive(true);

            }

            //添加未显示玩家
            for (int i = 0; i < message.Gamers.Count; i++)
            {
                //如果服务端发来了默认空GamerInfo 跳过
                GamerInfo gamerInfo = message.Gamers[i];
                if (gamerInfo.UserID == 0)
                    continue;

                Gamer gamer = ETModel.ComponentFactory.Create<Gamer, long>(gamerInfo.UserID);
                landRoomComponent.AddGamer(gamer, i);
            }

            await ETTask.CompletedTask;
        }
    }
}