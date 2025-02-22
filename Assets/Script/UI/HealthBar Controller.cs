using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    [Header("Elements")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    private VisualElement defenseElement;

    private Label defenseLabel;

    private VisualElement buffElement;
    private Label buffTurn;

    [Header("buff图片")]
    public Sprite buffSprite;
    public Sprite debuffSprite;

    private Enemy enemy;
    private VisualElement intentSprite;
    private Label intentAmount;
    
    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        enemy = GetComponent<Enemy>();
    }
    private void OnEnable()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        //将UIToolkit移动到具体的坐标
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }

    //为了不运行游戏也能看到bar
    [ContextMenu("Get UI Position")]
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        //查找HealthBar节点
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = currentCharacter.MaxHp;
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);

        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseLabel = defenseElement.Q<Label>("DefenseAmount");

        defenseElement.style.display = DisplayStyle.None;

        buffElement = healthBar.Q<VisualElement>("Buff");
        buffTurn = buffElement.Q<Label>("BuffTurn");
        buffElement.style.display = DisplayStyle.None;

        intentSprite = healthBar.Q<VisualElement>("Intent");
        intentAmount = intentSprite.Q<Label>("IntentAmount");
        intentSprite.style.display = DisplayStyle.None;
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        if(currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if(healthBar!=null)
        {
            healthBar.title = $"HP:{currentCharacter.currentHp}/{currentCharacter.MaxHp}";
            healthBar.value = currentCharacter.currentHp;

            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            var percentage = (float)currentCharacter.currentHp / (float)currentCharacter.MaxHp;
            if(percentage<0.3f){
                healthBar.AddToClassList("lowHealth");
            }
            else if(percentage<0.6f){
                healthBar.AddToClassList("mediumHealth");
            }
            else{
                healthBar.AddToClassList("highHealth");
            }

            defenseElement.style.display = currentCharacter.defense.currentValue > 0? DisplayStyle.Flex : DisplayStyle.None;
            defenseLabel.text = currentCharacter.defense.currentValue.ToString();

            //buff回合更新
            buffElement.style.display = currentCharacter.buffTurn.currentValue > 0? DisplayStyle.Flex : DisplayStyle.None;
            buffTurn.text = currentCharacter.buffTurn.currentValue.ToString();
            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1f? new StyleBackground(buffSprite) : new StyleBackground(debuffSprite);
        }
    }

    //在玩家回合开始前要标图标

    public void SetIntentElement(){
        intentSprite.style.display = DisplayStyle.Flex;
        intentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);

        //判断是否攻击
        var value = enemy.currentAction.effect.value;
        if(enemy.currentAction.effect.GetType() == typeof(DamageEffect)){
            value = (int) math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }

        intentAmount.text = value.ToString();
    }

    public void HideIntentElement(){
        intentSprite.style.display = DisplayStyle.None;
    }
}
