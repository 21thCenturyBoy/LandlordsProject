﻿using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 出牌缓存组件
    /// </summary>
    public class DeskCardsCacheComponent : Component
    {
        //出牌缓存
        public readonly List<Card> library = new List<Card>();

        //地主牌
        public readonly List<Card> LordCards = new List<Card>();

        //出牌缓存的总牌数
        public int CardsCount { get { return this.library.Count; } }

        //当前最大牌型
        public CardsType Rule { get; set; }

        //牌桌上最小的牌
        public int MinWeight { get { return (int)this.library[0].CardWeight; } }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            library.Clear();
            LordCards.Clear();
            Rule = CardsType.None;
        }
    }
}