using System;

namespace ETModel
{
    /// <summary>
    /// 参考Unit类
    /// </summary>
    public partial class Card : IEquatable<Card>
    {
        public static Card Create(int weight, int suits)
        {
            //TODO 以后改成对象池引用
            Card card = new Card();
            card.CardWeight = weight;
            card.CardSuits = suits;
            return card;
        }

        public bool Equals(Card other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CardWeight == other.CardWeight && CardSuits == other.CardSuits;
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
                return (CardWeight * 397) ^ CardSuits;
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