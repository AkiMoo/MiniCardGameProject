using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;

    private CardDataSO currentCardData;

    public CardManager cardManager;
    public GameObject deckCheckPanel;

    private List<Button> cardButtons = new();
    private Button confrimButton;
    private Button backToMapButton;
    private Button deckCheck;

    [Header("广播")]
    public ObjectEventSO pickFinishEvent;
    public ObjectEventSO loadMapEvent;

    private void OnEnable()
    {
        rootElement  = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q("Container");
        confrimButton = rootElement.Q<Button>("ConfirmButton");
        backToMapButton = rootElement.Q<Button>("BackToMap");
        deckCheck = rootElement.Q<Button>("DeckCheck");

        confrimButton.clicked += OnConfirmButtonClick;
        backToMapButton.clicked += OnBackToMapButtonClicked;
        deckCheck.clicked += OnDeckCheckClicked;
        //这里我想要按照房间类型生成不一样的卡牌，已完成
        for(int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            if(SceneManager.GetActiveScene().name == "Treasure"){
                data = cardManager.GetTreasureCardData();
            }
            InitCard(card, data);
            //这里可以调整卡片大小的显示
            //card.style.height = 330;

            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);
            cardButton.clicked += () => OnCardButtonClick(cardButton, data);
        }
    }
    private void OnDeckCheckClicked()
    {
        deckCheckPanel.SetActive(true);
    }
    private void OnConfirmButtonClick(){
        cardManager.UnlockCard(currentCardData);
        pickFinishEvent.RaiseEvent(null, this);
    }

    private void OnCardButtonClick(Button cardButton, CardDataSO cardData){
        currentCardData = cardData;
        //Debug.Log("选中卡牌：" + currentCardData.cardName);
        for(int i = 0; i < cardButtons.Count; i++)
        {
            if(cardButtons[i] == cardButton){
                cardButtons[i].SetEnabled(false);
            }
            else{
                cardButtons[i].SetEnabled(true);
            }
        }
    }
    private void OnBackToMapButtonClicked()
    {
        pickFinishEvent.RaiseEvent(null, this);
        loadMapEvent.RaiseEvent(null, this);
    }
    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        //存储传入的卡
        card.dataSource = cardData;

        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("Mana");
        var cardName = card.Q<Label>("CardName");
        var cardType = card.Q<Label>("CardType");
        var cardDescription = card.Q<Label>("CardDescription");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardCost.text = cardData.cardCost.ToString();
        cardName.text = cardData.cardName;
        cardDescription.text = cardData.cardDescription;
        cardType.text = cardData.cardType switch
        {
            CardType.Ability => "能力",
            CardType.Defence => "防御",
            CardType.Attack => "攻击",
            CardType.Heal => "治疗",
            CardType.Event => "事件",
            _ => "Unknown"
        };
    }
}
