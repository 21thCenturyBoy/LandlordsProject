using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 缓存组件系统
    /// </summary>
    public static class DeskCardsCacheComponentSystem
    {
        /// <summary>
        /// 获取总权值
        /// </summary>
        public static int GetTotalWeight(this DeskCardsCacheComponent self)
        {
            //return CardHelper.GetWeight(self.library.ToArray(), self.Rule);
            return 0;
        }

        /// <summary>
        /// 获取牌桌所有牌
        /// </summary>
        public static Card[] GetAll(this DeskCardsCacheComponent self)
        {
            return self.library.ToArray();
        }

        /// <summary>
        /// 发牌
        /// 玩家的牌都是由手牌组件发牌的，牌库中只有一开始的地主牌是从牌库发牌（借用）
        /// </summary>
        public static Card Deal(this DeskCardsCacheComponent self)
        {
            Card card = self.library[self.CardsCount - 1];
            self.library.Remove(card);
            return card;
        }

        /// <summary>
        /// 向牌库中添加牌
        /// </summary>
        public static void AddCard(this DeskCardsCacheComponent self, Card card)
        {
            self.library.Add(card);
        }

        /// <summary>
        /// 清空牌桌
        /// </summary>
        public static void Clear(this DeskCardsCacheComponent self)
        {
            DeckComponent deck = self.GetParent<Entity>().GetComponent<DeckComponent>();
            while (self.CardsCount > 0)
            {
                Card card = self.library[self.CardsCount - 1];
                self.library.Remove(card);
                deck.AddCard(card);
            }

            self.Rule = CardsType.None;
        }
    }
}