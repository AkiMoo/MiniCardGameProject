using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeckCheckPanel : MonoBehaviour
{
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer,cardContainer2,cardContainer3;

    public CardLibrarySO currentCardLibrary;

    public GameObject deckCheckPanel, gameplayPanel;
    private Button backButton;
    private void OnEnable()
    {
        Debug.Log("牌库展示");
        rootElement  = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q("Container");
        cardContainer2 = rootElement.Q("Container2");
        cardContainer3 = rootElement.Q("Container3");
        backButton = rootElement.Q<Button>("Back");
        backButton.clicked += OnBackButtonClicked;
        List<CardDataSO> tempStorage = new List<CardDataSO>();
        foreach(var item in currentCardLibrary.cardList)
        {
            for(int i = 0; i < item.amount; i++)
            {
                tempStorage.Add(item.cardData);
            }
        }
        Debug.Log($"总容量"+tempStorage.Count);
        for(int j =0 ; j < tempStorage.Count; j++)
        {
            var card = cardTemplate.Instantiate();
            InitCard(card, tempStorage[j]);
            card.style.height = 5;
            if(cardContainer.childCount < 10)
            {
                cardContainer.Add(card);
            }
            else if(cardContainer2.childCount >= 10)
            {
                cardContainer3.Add(card);
            }
            else{
                cardContainer2.Add(card);
            }
            //发现即使在这里重复Add两次也不行，是模版的关系吗？
        }
        //记录，为什么这种方法不可以加入重复元素
        // foreach(var item in currentCardLibrary.cardList)
        // {
        //     var card = cardTemplate.Instantiate();
        //     InitCard(card, item.cardData);
        //     for(int i = 0; i < item.amount; i++)
        //     {
        //         cardContainer.Add(card);
        //     }            
        // }
        
    }

    private void OnBackButtonClicked()
    {
        Debug.Log("返回游戏");
        deckCheckPanel.SetActive(false);
    }
    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        //存储传入的卡
        card.dataSource = cardData;
        Debug.Log("卡牌生成");
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
