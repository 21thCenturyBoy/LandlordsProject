﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class HandCardsComponentAwakeSystem : AwakeSystem<HandCardsComponent, GameObject>
    {
        public override void Awake(HandCardsComponent self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class HandCardsComponent : Component
    {
        public const string HANDCARD_NAME = "HandCard";
        public const string PLAYCARD_NAME = "PlayCard";

        private readonly Dictionary<string, GameObject> cardsSprite = new Dictionary<string, GameObject>();
        private readonly List<Card> handCards = new List<Card>();
        private readonly List<Card> playCards = new List<Card>();

        private GameObject _poker;
        private GameObject _handCards;
        private Text _pokerNum;

        public GameObject Panel { get; private set; }
        public Identity AccessIdentity { get; set; }

        public void Awake(GameObject panel)
        {
            this.Panel = panel;
            _poker = this.Panel.Get<GameObject>("Poker");
            _handCards = this.Panel.Get<GameObject>("HandCards");
            _pokerNum = _poker?.GetComponentInChildren<Text>();

            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{HANDCARD_NAME}.unity3d");
            resourcesComponent.LoadBundle($"{PLAYCARD_NAME}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.ATLAS_NAME}.unity3d");
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            ClearHandCards();
            ClearPlayCards();
        }

        /// <summary>
        /// 显示玩家游戏UI
        /// </summary>
        public void Appear()
        {
            _poker?.SetActive(true);
            _handCards?.SetActive(true);
        }

        /// <summary>
        /// 隐藏玩家游戏UI
        /// </summary>
        public void Hide()
        {
            _poker?.SetActive(false);
            _handCards?.SetActive(false);
        }

        /// <summary>
        /// 获取卡牌精灵
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public GameObject GetSprite(Card card)
        {
            GameObject cardSprite;
            if (cardsSprite.TryGetValue(card.GetName(), out cardSprite))
            {
                return cardSprite;
            }
            Log.Debug("没找到这张卡牌！" + card.GetName());
            return cardSprite;
        }

        /// <summary>
        /// 设置手牌数量
        /// </summary>
        /// <param name="num"></param>
        public void SetHandCardsNum(int num)
        {
            _pokerNum.text = num.ToString();
        }

        /// <summary>
        /// 添加多张牌
        /// </summary>
        /// <param name="cards"></param>
        public void AddCards(Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                AddCard(cards[i]);
            }
            CardsSpriteUpdate(handCards, 50.0f);
        }

        /// <summary>
        /// 出牌后更新玩家手牌
        /// </summary>
        /// <param name="cards"></param>
        public void PopCards(Card[] cards)
        {
            //ClearPlayCards();  //造成重复删除，先注释掉

            for (int i = 0; i < cards.Length; i++)
            {
                PopCard(cards[i]);
                Log.Debug("更新一张牌：" + cards[i].GetName());
            }
            CardsSpriteUpdate(playCards, 25.0f);
            CardsSpriteUpdate(handCards, 50.0f);

            //同步剩余牌数
            GameObject poker = this.Panel.Get<GameObject>("Poker");
            if (poker != null)
            {
                Text pokerNum = poker.GetComponentInChildren<Text>();
                pokerNum.text = (int.Parse(pokerNum.text) - cards.Length).ToString();
            }
        }

        /// <summary>
        /// 清空手牌
        /// </summary>
        public void ClearHandCards()
        {
            ClearCards(handCards);
        }

        /// <summary>
        /// 清空出牌
        /// </summary>
        public void ClearPlayCards()
        {
            ClearCards(playCards);
        }

        /// <summary>
        /// 卡牌精灵更新
        /// </summary>
        public void CardsSpriteUpdate(List<Card> cards, float interval)
        {
            if (cards.Count == 0)
            {
                return;
            }

            Sort(cards);

            float width = GetSprite(cards[0]).GetComponent<RectTransform>().sizeDelta.x;
            float startX = -((cards.Count - 1) * interval) / 2;
            for (int i = 0; i < cards.Count; i++)
            {
                RectTransform rect = GetSprite(cards[i]).GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(startX + (i * interval), rect.anchoredPosition.y);
            }
        }

        /// <summary>
        /// 清空卡牌
        /// </summary>
        /// <param name="cards"></param>
        private void ClearCards(List<Card> cards)
        {
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                Card card = cards[i];
                Log.Debug("删除卡牌" + card.GetName());
                GameObject cardSprite = cardsSprite[card.GetName()];
                cardsSprite.Remove(card.GetName());
                cards.Remove(card);
                UnityEngine.Object.Destroy(cardSprite);
            }
        }

        /// <summary>
        /// 卡牌排序
        /// </summary>
        /// <param name="cards"></param>
        private void Sort(List<Card> cards)
        {
            CardHelper.Sort(cards);

            //卡牌精灵层级排序
            //Log.Debug("重设卡牌精灵层级");
            for (int i = 0; i < cards.Count; i++)
            {
                //Log.Debug("卡牌精灵" + cards[i].GetName());
                GetSprite(cards[i]).transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// 添加卡牌
        /// </summary>
        /// <param name="card"></param>
        private void AddCard(Card card)
        {
            GameObject handCardSprite = CreateCardSprite(HANDCARD_NAME, card.GetName(), this.Panel.Get<GameObject>("HandCards").transform);
            handCardSprite.GetComponent<HandCardSprite>().Poker = card;
            cardsSprite.Add(card.GetName(), handCardSprite);
            handCards.Add(card);
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="card"></param>
        private void PopCard(Card card)
        {
            //移除手牌
            //因为对象并不是完全一致，不能用Contains来查找
            foreach (var a in handCards)
            {
                if (a.Equals(card))
                {
                    Log.Debug("pop删除卡牌" + a.GetName());
                    GameObject handCardSprite = GetSprite(a);
                    cardsSprite.Remove(a.GetName());
                    handCards.Remove(a);
                    UnityEngine.Object.Destroy(handCardSprite);
                    break;
                }
            }

            GameObject playCardSprite = CreateCardSprite(PLAYCARD_NAME, card.GetName(), this.Panel.Get<GameObject>("PlayCards").transform);
            cardsSprite.Add(card.GetName(), playCardSprite);
            playCards.Add(card);
        }

        /// <summary>
        /// 创建卡牌精灵
        /// </summary>
        private GameObject CreateCardSprite(string prefabName, string cardName, Transform parent)
        {
            GameObject cardSpritePrefab = (GameObject)Game.Scene.GetComponent<ResourcesComponent>().GetAsset($"{prefabName}.unity3d", prefabName);
            GameObject cardSprite = UnityEngine.Object.Instantiate(cardSpritePrefab);

            cardSprite.name = cardName;
            cardSprite.layer = LayerMask.NameToLayer("UI");
            cardSprite.transform.SetParent(parent.transform, false);

            Sprite sprite = CardHelper.GetCardSprite(cardName);
            cardSprite.GetComponent<Image>().sprite = sprite;

            return cardSprite;
        }
    }
}