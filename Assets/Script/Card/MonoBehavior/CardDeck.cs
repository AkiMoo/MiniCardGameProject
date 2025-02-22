using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Unity.VisualScripting;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPosition;
    //抽牌堆
    private List<CardDataSO> cardDeck = new();
    //弃牌堆
    private List<CardDataSO> discardDeck = new();
    //手牌
    private List<Card> handCardObjects = new();

    [Header("事件广播")]
    public IntEventSO drawCardEvent;
    public IntEventSO discardCardEvent;
    public ObjectEventSO gameOverEvent;
    //测试用
    private void Start()
    {
        InintializeDeck();
        discardDeck.Clear();
    }
    public void InintializeDeck()
    {
        cardDeck.Clear();
        foreach(var item in cardManager.currentCardLibrary.cardList)
        {
            for(int i = 0; i < item.amount; i++)
            {
                cardDeck.Add(item.cardData);
            }
        }
        shuffleDeck();
    }
    public void NewTurnDrawCards()
    {
        DrawCard(2);
    }
    [ContextMenu("Draw Card")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    public void DrawCard(int amount)
    {
        if(cardDeck.Count < amount)
        {
            //败北或者将弃牌堆洗切
            //gameOverEvent.RaiseEvent(null,this);
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.TakeDamage(100000);
            return;
        }
        for(int i = 0; i < amount; i++)
        {
            
            CardDataSO currentCardData = cardDeck[0];
            cardDeck.RemoveAt(0);
            //抽牌更新数值
            drawCardEvent.RaiseEvent(cardDeck.Count,this);
            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            //从卡组把卡加入手牌的坐标变化
            card.transform.position = deckPosition;
            handCardObjects.Add(card);
            //每张卡渲染的延迟,
            var delay = i*0.2f;
            SetLayout(delay);
        }
    }
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjects.Remove(card);
        //对象回收进池子，我的玩法需要弃牌堆，暂定不回收
        cardManager.DiscardCard(card.gameObject);
        discardCardEvent.RaiseEvent(discardDeck.Count,this);
        //手牌重新排列
        SetLayout(0);
    }

    public void SetCardLayoutAfterSkill()
    {
        SetLayout(0);
    }

    private void SetLayout(float delay)
    {
        for(int i=0; i<handCardObjects.Count; i++)
        {
            Card currentCard = handCardObjects[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjects.Count);
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);
            //移动结束后才能感应鼠标

            //卡牌能量判断
            currentCard.UpdateCardState();

            currentCard.isAnimating = true;
            //0.2 和 0.5 是秒数
            currentCard.transform.DOScale(0.4f*Vector3.one, 0.2f).SetDelay(delay).onComplete = () => {
                //之前的牌已经变成了给定的大小，只有新生成的那些scale设置成0的牌会运行这句代码
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => {
                    currentCard.isAnimating = false;
                };
                //下面应该是为了扇形布局而准备的
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            //设置卡牌的叠层
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    private void shuffleDeck()
    {
        //在我的游戏设计里洗牌可能不一定涉及弃牌堆清理
        //discardDeck.Clear();
        drawCardEvent.RaiseEvent(cardDeck.Count,this);
        discardCardEvent.RaiseEvent(discardDeck.Count,this);

        for(int i=0; i<cardDeck.Count; i++)
        {
            CardDataSO temp = cardDeck[i];
            int randomIndex = Random.Range(i, cardDeck.Count);
            cardDeck[i] = cardDeck[randomIndex];
            cardDeck[randomIndex] = temp;
        }
    }
    public void OnPlayerTurnEnd()
    {
        //这里全弃掉是为了测试 试试不弃置
        // for(int i =0;i<handCardObjects.Count;i++)
        // {
        //     discardDeck.Add(handCardObjects[i].cardData);
        //     cardManager.DiscardCard(handCardObjects[i].gameObject);
        // }
        // handCardObjects.Clear();
        // discardCardEvent.RaiseEvent(discardDeck.Count,this);
    }

    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardObjects)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardObjects.Clear();
        InintializeDeck();
    }
}
