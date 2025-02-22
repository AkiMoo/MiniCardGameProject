using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopPanel : MonoBehaviour
{
    public Player player;
    private VisualElement rootVisualElement;
    public VisualTreeAsset goodsTemplate;
    private VisualElement goodsContainer;
    private Button DealButton;
    private Button backToButton;
    public GoodManager goodManager;
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO loadMapEvent;
    public GameObject deckCheckPanel;
    private Button deckCheck;
    private List<Button> goodsButtons = new();
    private GoodDataSO currentGoodData;
    private Label moneyAmount;

    [Header("广播")]
    public ObjectEventSO pickFinishEvent;
    public IntEventSO MoneySpend;
    public ObjectEventSO GoodEffect;

    private void OnEnable()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        DealButton = rootVisualElement.Q<Button>("DealButton");
        backToButton = rootVisualElement.Q<Button>("BackToMap");
        moneyAmount = rootVisualElement.Q<Label>("MoneyAmount");
        deckCheck = rootVisualElement.Q<Button>("DeckCheck");
        
        DealButton.clicked += OnDealButtonClicked;
        backToButton.clicked += OnBackToMapButtonClicked;
        deckCheck.clicked += OnDeckCheckClicked;

        goodsContainer = rootVisualElement.Q("Container");
        moneyAmount.text = player.currentMoney.ToString();
        for(int i = 0; i < 2; i++)
        {
            var good = goodsTemplate.Instantiate();
            var data = goodManager.GetGoodData();
            InitGood(good, data);
            //这里可以调整卡片大小的显示
            //card.style.height = 330;

            var goodsButton = good.Q<Button>("Good");
            goodsContainer.Add(good);
            goodsButtons.Add(goodsButton);
            goodsButton.clicked += () => OnGoodButtonClick(goodsButton, data);
        }

    }
    
    private void OnDeckCheckClicked()
    {
        deckCheckPanel.SetActive(true);
    }
    private void OnGoodButtonClick(Button goodsButton, GoodDataSO goodData){
        currentGoodData = goodData;
        //Debug.Log("选中物品：" + currentCardData.cardName);
        for(int i = 0; i < goodsButtons.Count; i++)
        {
            if(goodsButtons[i] == goodsButton){
                goodsButtons[i].SetEnabled(false);
            }
            else{
                goodsButtons[i].SetEnabled(true);
            }
        }
    }
    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }
    public void OnDealButtonClicked()
    {
        if(currentGoodData!= null){

            if(player!=null&&player.currentMoney>=currentGoodData.price)
            {
                goodManager.ExecuteGoodEffect(currentGoodData, player, player);
                //此处不知道为何RaiseEvent没能触发player的MoneyChange事件，问题保留
                player.currentMoney -= currentGoodData.price;
                MoneySpend.RaiseEvent(currentGoodData.price, this);
                moneyAmount.text = player.currentMoney.ToString();
                Debug.Log("购买了"+currentGoodData.goodName);
            }
            else{
                Debug.Log("金币不足或Player不存在");
            }
            //pickFinishEvent.RaiseEvent(null, this);
            
        }
    }
    public void InitGood(VisualElement good, GoodDataSO goodData)
    {
        //存储传入的宝物
        good.dataSource = goodData;

        var goodSpriteElement = good.Q<VisualElement>("GoodSprite");
        var goodPrice = good.Q<Label>("Price");
        var goodName = good.Q<Label>("GoodName");
        var goodDescription = good.Q<Label>("GoodDescription");

        goodSpriteElement.style.backgroundImage = new StyleBackground(goodData.goodImage);
        goodPrice.text = goodData.price.ToString();
        goodName.text = goodData.goodName;
        goodDescription.text = goodData.goodDescription;
    }
}
