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
                uiRoom.GameObject.Get<GameObject>("Ready").SetActive(true);
            }

            //服务端发过来3个GamerInfo 本地玩家进入房间的顺序
            int localIndex = -1;
            for (int i = 0; i < message.Gamers.Count; i++)
            {
                if (message.Gamers[i].UserID == LandRoomComponent.LocalGamer.UserID)
                {
                    //得出本地玩家是第几个进入房间，可能是0,1,2
                    localIndex = i;
                }
            }

            //添加进入房间的玩家，判定座位位置
            for (int i = 0; i < message.Gamers.Count; i++)
            {
                //如果服务端发来了默认空GamerInfo 跳过
                GamerInfo gamerInfo = message.Gamers[i];
                if (gamerInfo.UserID == 0)
                    continue;
                //如果这个ID的玩家不在桌上
                if (landRoomComponent.GetGamer(gamerInfo.UserID) == null)
                {
                    Gamer gamer = ETModel.ComponentFactory.Create<Gamer, long>(gamerInfo.UserID);

                    // localIndex + 1 指本好玩家后进入的下一个玩家
                    // 不论本地玩家是第几个进入房间，都放在1号位（下边）
                    if ((localIndex + 1) % 3 == i)
                    {
                        //玩家在本地玩家右边2号位
                        landRoomComponent.AddGamer(gamer, 2);
                    }
                    else
                    {
                        //玩家在本地玩家左边0号位
                        landRoomComponent.AddGamer(gamer, 0);
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}