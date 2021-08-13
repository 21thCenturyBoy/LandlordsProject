namespace ETModel
{
    /// <summary>
    /// 参考Unit类
    /// </summary>
    public partial class Card
    {
        public static Card Create(int weight, int suits)
        {
            Card card = new Card();
            card.CardWeight = weight; //点数
            card.CardSuits = suits; //花色
            return card;
        }

        public bool Equals(Card other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return cardWeight_ == other.cardWeight_ && cardSuits_ == other.cardSuits_;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Card)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (cardWeight_ * 397) ^ cardSuits_;
            }
        }

        public static bool operator ==(Card left, Card right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !Equals(left, right);
        }
        //public bool Equals(Card other)
        //{
        //    return this.CardWeight == other.CardWeight && this.CardSuits == other.CardSuits;
        //}
        public override string ToString()
        {
            return $"[{nameof(CardWeight)}: {CardWeight}, {nameof(CardSuits)}: {CardSuits}]";
        }
        /// <summary>
        /// 获取卡牌名
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.CardSuits == (int)Suits.None ? ((Weight)this.CardWeight).ToString() : $"{((Suits)this.CardSuits).ToString()}{((Weight)this.CardWeight).ToString()}";
        }
    }

}