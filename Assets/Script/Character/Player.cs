using Unity.VisualScripting;
using UnityEngine;

public class Player : CharacterBase
{
    [Header("玩家数据")]
    //IntVariable类型都是为了读取数值，方便序列化保存
    public IntVariable mana;
    public int maxMana;
    //不使用序列化保存的话不会生效
    public IntVariable money;

    public string Skill1Name;
    public string Skill2Name;
    public string Skill3Name;
    public string Skill1Description;
    public string Skill2Description;
    public string Skill3Description;

    public IntEventSO moneyChangeEvent;
    public IntEventSO manaChangeEvent;
    public int currentMoney{get=>money.currentValue; set=>money.SetValue(value);}
    public int currentMana{get=>mana.currentValue; set=>mana.SetValue(value);}
    private void OnEnable()
    {
        mana.maxValue = maxMana;
        currentMana = 2;
        //currentMoney = money.currentValue;
    }

    public void NewTurn()
    {
        moneyChangeEvent.RaiseEvent(currentMoney, this);
        if(currentMana < maxMana)
            currentMana++;
    }
    /// <summary>
    /// 新游戏开始需要重置的内容
    /// TODO: 周目续档
    /// </summary>
    public void NewGame()
    {
        currentHp = maxHp;
        currentMoney = 30;
        isDead = false;
        maxMana = 5;
        hp.maxValue = 60;
        baseStrength = 1;
        buffTurn.SetValue(0);
        NewTurn();
    }

    /// <summary>
    /// 法力处理相关
    /// </summary>
    /// <param name="value"></param>
    public void UseMana(int value)
    {
        currentMana -= value;
        if(currentMana <= 0)
        {
            currentMana = 0;
        }
    }
    public void PlusMana(int value)
    {
        currentMana += value;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        manaChangeEvent.RaiseEvent(currentMana, this);
    }
    public void BreakManaLimit(int value)
    {
        maxMana += value;
    }

    /// <summary>
    /// 钱币相关处理
    /// </summary>
    /// <param name="value"></param>
    public void PlusMoney(int value)
    {
        currentMoney += value;
        //moneyChangeEvent.RaiseEvent(currentMoney, this);
    }

    public void UseMoney(int value)
    {
        Debug.Log("买东西");
        if(currentMoney >= value)
        {
            currentMoney -= value;
        }
        else{
            //消费失败
            return;
        }
    }
}
