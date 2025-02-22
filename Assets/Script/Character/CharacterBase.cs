using UnityEngine;
using UnityEngine.Events;
//敌人和角色都会继承的基类，将通用的属性放到这里
public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffTurn;

    //使用SetValue的好处是，一旦调用将会启动事件

    public int currentHp{get=>hp.currentValue; set=>hp.SetValue(value);}
    public int MaxHp {get=>hp.maxValue;}
    //public float ATK;
    protected Animator animator;
    public bool isDead;
    public GameObject buff;
    public GameObject debuff;

    public float baseStrength = 1;
    //这里不固定数值的话要怎么写？
    public float StrengthEffect = 0.5f;

    [Header("广播")]
    public ObjectEventSO characterDeadEvent;
    public FloatEventSO strengthChangeEvent;

    //[Header("Boardcast")]
    //public ObjectEventSO characterDeadEvent;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        currentHp = maxHp;
        buffTurn.currentValue = 0;
        ResetDefense();
    }
    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue)>=0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue)>=0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);
        if(currentHp>currentDamage)
        {
            currentHp -= currentDamage;
            Debug.Log("current hp: " + currentHp);
            animator.SetTrigger("hit");
        }
        else{
            currentHp = 0;
            isDead = true;
            characterDeadEvent?.RaiseEvent(this, this);
        }
    }

    public void UpdateDefense(int value)
    {
        var finaldef = defense.currentValue + value;
        defense.SetValue(finaldef);
    }
    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void UpdateHealth(int value){
        if(hp.currentValue + value <= hp.maxValue)
        {
            currentHp += value;
        }
        else{
            currentHp = hp.maxValue;
        }
        buff.SetActive(true);
    }

    public void ResetHealth()
    {
        hp.SetValue(maxHp);
    }

    public void UpdateMaxHp(int value)
    {
        hp.maxValue += value;
    }
    public void UpdateBaseStrength(int value)
    {
        // 因为卡片buff是要算回合的，如果跨越了战斗回合，这个变量就会有影响，先暂时不用乘算
        // float multiple = 1 + (value * 0.01f);
        // baseStrength *= multiple;
        float plusAmount = value * 0.01f;
        baseStrength += plusAmount;
    }
    //positive用于区分敌我
    public void SetupStrength(int turn, bool isPositive)
    {
        if(isPositive)
        {
            Debug.Log("base strength: " + baseStrength);
            Debug.Log("strength effect: " + StrengthEffect);
            float newStrength = baseStrength + StrengthEffect;
            Debug.Log("new strength: " + newStrength);
            baseStrength = newStrength;
            Debug.Log("base strength: " + baseStrength);
            buff.SetActive(true);
            strengthChangeEvent?.RaiseEvent(baseStrength, this);
        }
        else{
            Debug.Log("isHere?");
            float newStrength = baseStrength - StrengthEffect;
            baseStrength = newStrength;
            debuff.SetActive(true);
        }
        buffTurn.currentValue = turn;
    }
    //回合转换事件函数
    public void UpdateBuffTurn()
    {
        //如果不加这个的话似乎就会先减到-1，出问题
        if(buffTurn.currentValue > 0){
            buffTurn.SetValue(buffTurn.currentValue - 1);
            if(buffTurn.currentValue <= 0){
                buffTurn.SetValue(0);
                baseStrength -= StrengthEffect;
                strengthChangeEvent?.RaiseEvent(baseStrength, this);
            }
        }
    }
}
