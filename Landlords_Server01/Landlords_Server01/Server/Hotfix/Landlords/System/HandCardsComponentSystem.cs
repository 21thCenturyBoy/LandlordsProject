using ETModel;

namespace ETHotfix
{
    public static class HandCardsComponentSystem
    {
        /// <summary>
        /// 获取所有手牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Card[] GetAll(this HandCardsComponent self)
        {
            return self.library.ToArray();
        }

        /// <summary>
        /// 向手牌中添加牌
        /// </summary>
        /// <param name="card"></param>
        public static void AddCard(this HandCardsComponent self, Card card)
        {
            self.library.Add(card);
        }

        /// <summary>
        /// 出牌后从手牌移除
        /// </summary>
        /// <param name="self"></param>
        /// <param name="card"></param>
        public static void PopCard(this HandCardsComponent self, Card card)
        {
            self.library.Remove(card);
        }

    }
}