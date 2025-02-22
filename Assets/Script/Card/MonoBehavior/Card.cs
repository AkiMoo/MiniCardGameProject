using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Properties")]
    public SpriteRenderer cardSprite;
    public TextMeshPro cardName;
    public TextMeshPro cardCost;
    public TextMeshPro cardDescription;
    public TextMeshPro number;
    public SpriteRenderer typeSprite;
    public TextMeshPro type;
    public CardDataSO cardData;
    [Header("Original Data")]
    //用于鼠标滑入滑出事件，选中卡牌
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;//操作中通过改变layer层数让卡片浮空
    public bool isAnimating;//动画触发
    public bool isAvailiable;//是否可用
    
    public Player player;
    [Header("Boardcast Event")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent;

    private void Start()
    {
        Init(cardData);
    }
    public void Init(CardDataSO data)
    {
        cardData = data;
        if(cardData==null){
            Debug.Log("CardData is null");
            return;
        }
        cardCost.text = cardData.cardCost.ToString();
        cardSprite.sprite = data.cardImage;
        cardName.text = data.cardName;
        cardDescription.text = data.cardDescription;
        type.text = data.cardType switch
        {
            CardType.Ability => "能力",
            CardType.Defence => "防御",
            CardType.Attack => "攻击",
            CardType.Heal => "治疗",
            CardType.Event => "事件",
            _ => "Unknown"
        };
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        this.originalPosition = position;
        this.originalRotation = rotation;
        this.originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAnimating)
        {
            return;
        }
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;//保证层数足够高
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating)
        {
            return;
        }
        ResetCardTransform();
    }
    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        costEvent.RaiseEvent(cardData.cardCost, this);
        discardCardEvent.RaiseEvent(this, this);
        foreach (var effect in cardData.effects)
        {
            effect.Execute(from, target);
        }
        //Debug.Log("Card ExecuteEffect");
    }

    public void UpdateCardState()
    {

        isAvailiable = cardData.cardCost <= player.currentMana;
        cardCost.color = isAvailiable? Color.green : Color.red;
        //此处可以实现UI逻辑，例如可用卡片周围发光之类的
    }
}
