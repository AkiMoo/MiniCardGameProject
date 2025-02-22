using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class GameplayPanel : MonoBehaviour
{
    [Header("事件广播")]
    public ObjectEventSO playerTurnEndEvent;
    public ObjectEventSO AfterSkillEvent;
    private VisualElement rootElement;
    private CharacterBase target;
    private Player player;
    private Label manaAmmount, drawAmount, discardAmount, turnlabel, moneyAmount, baseStrength;

    private Label skill1CostLabel, skill1DescriptionLabel, skill2CostLabel, skill2DescriptionLabel, skill3CostLabel, skill3DescriptionLabel;
    private Button endTurnButton;

    private Button skill1Button, skill2Button, skill3Button;
    private Button deckCheck, HelpButton;

    public GameObject gameplayPanel, deckCheckPanel, helpPanel;
    //地图查看暂时先不做
    //public Button mapCheck;

    public Effect skill1Effect, skill2Effect, skill3Effect;
    

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        //添加UI元素
        manaAmmount = rootElement.Q<Label>("ManaAmount");
        drawAmount = rootElement.Q<Label>("DrawAmount");
        discardAmount = rootElement.Q<Label>("DiscardAmount");
        moneyAmount = rootElement.Q<Label>("MoneyAmount");
        turnlabel = rootElement.Q<Label>("TurnLabel");
        baseStrength = rootElement.Q<Label>("BaseStrengthText");
        endTurnButton = rootElement.Q<Button>("TurnChange");
        skill1Button = rootElement.Q<Button>("Skill1");
        skill2Button = rootElement.Q<Button>("Skill2");
        skill3Button = rootElement.Q<Button>("Skill3");

        skill1CostLabel = rootElement.Q<Label>("Skill1Cost");
        skill1DescriptionLabel = rootElement.Q<Label>("Skill1Description");
        skill2CostLabel = rootElement.Q<Label>("Skill2Cost");
        skill2DescriptionLabel = rootElement.Q<Label>("Skill2Description");
        skill3CostLabel = rootElement.Q<Label>("Skill3Cost");        
        skill3DescriptionLabel = rootElement.Q<Label>("Skill3Description");

        deckCheck = rootElement.Q<Button>("DeckCheck");
        HelpButton = rootElement.Q<Button>("HelpButton");

        //绑定按钮事件
        endTurnButton.clicked += OnEndTurnButtonClicked;
        skill1Button.clicked += OnSkill1ButtonClicked;
        skill2Button.clicked += OnSkill2ButtonClicked;
        skill3Button.clicked += OnSkill3ButtonClicked;

        deckCheck.clicked += OnDeckCheckClicked;
        HelpButton.clicked += OnHelpButtonClicked;

        

        //初始化启动显示的内容
        discardAmount.text = "0";
        drawAmount.text = "0";
        manaAmmount.text = "0";
        moneyAmount.text = "0";
        baseStrength.text = "1";
        turnlabel.text = "GAME START!";

        skill1CostLabel.text = " "; 
        skill1DescriptionLabel.text = " ";
        skill2CostLabel.text = " ";
        skill2DescriptionLabel.text = " ";
        skill3CostLabel.text = " ";
        skill3DescriptionLabel.text = " ";
        
    }

    private void OnHelpButtonClicked()
    {
        helpPanel.SetActive(true);
    }

    private void OnDeckCheckClicked()
    {
        deckCheckPanel.SetActive(true);
    }

    public void catchTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CharacterBase>();
    }

    private void OnEndTurnButtonClicked()
    {
        playerTurnEndEvent.RaiseEvent(null,this);
    }
    //TODO:下面的技能还是要区分技能类型再进行目标，目前先这样尝试绑定
    private void OnSkill1ButtonClicked()
    {
        catchTarget();
        if(player.currentMana < 1)
        {
            return;
        }
        player.UseMana(1);
        skill1Effect.Execute(player, target);
        AfterSkillEvent.RaiseEvent(null,this);
    }
    private void OnSkill2ButtonClicked()
    {
        catchTarget();
        if(player.currentMana < 2)
        {
            return;
        }
        player.UseMana(2);
        skill2Effect.Execute(player, target);
        AfterSkillEvent.RaiseEvent(null,this);
    }
    private void OnSkill3ButtonClicked()
    {
        catchTarget();
        if(player.currentMana < 3)
        {
            return;
        }
        player.UseMana(3);
        skill3Effect.Execute(player, target);
        AfterSkillEvent.RaiseEvent(null,this);
    }
    public void UpdateManaAmount(int amount)
    {
        manaAmmount.text = amount.ToString();
    }
    public void UpdateDrawAmount(int amount)
    {
        drawAmount.text = amount.ToString();
    }
    public void UpdateDiscardAmount(int amount)
    {
        discardAmount.text = amount.ToString();
    }
    public void UpdateMoneyAmount(int amount)
    {
        moneyAmount.text = amount.ToString();
    }
    public void UpdateBaseStrength(float amount)
    {
        baseStrength.text = amount.ToString();
    }
    public void UpdateSkillInformation()
    {
        catchTarget();
        baseStrength.text = player.baseStrength.ToString();
        skill1CostLabel.text = player.Skill1Name.ToString();
        skill1DescriptionLabel.text = player.Skill1Description.ToString();
    
        skill2CostLabel.text = player.Skill2Name.ToString();
        skill2DescriptionLabel.text = player.Skill2Description.ToString();

        skill3CostLabel.text = player.Skill3Name.ToString();
        skill3DescriptionLabel.text = player.Skill3Description.ToString();
    }
    //以下是回合转换显示
    public void OnEnemyTurnBgein()
    {
        endTurnButton.SetEnabled(false);
        turnlabel.text = "ENEMY TURN";
        turnlabel.style.color = new StyleColor(Color.red);
    }
    public void OnPlayerTurnBegin()
    {
        //catchTarget();
        
        endTurnButton.SetEnabled(true);
        turnlabel.text = "PLAYER TURN";
        turnlabel.style.color = new StyleColor(Color.white);
    }
}
