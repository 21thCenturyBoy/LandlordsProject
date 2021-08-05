using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class LandMatcherReduceOne_Handler : AMHandler<Actor_LandMatcherReduceOne_NTT>
    {
        protected override async ETTask Run(ETModel.Session session, Actor_LandMatcherReduceOne_NTT message)
        {
            Log.Debug("匹配玩家-1");

            await ETTask.CompletedTask;
        }
    }
}