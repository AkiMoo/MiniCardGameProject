using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionSO enemyActionSO;
    public EnemyAction currentAction;
    protected Player player;
    public EnemyAction Below50Action;
    public EnemyAction Below20Action;
    protected Enemy currentEnemy;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    // }

    public virtual void OnPlayerTurnBegin()
    {
        //意图可以根据当前HP行动 目前是随机形态
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentEnemy = GetComponent<Enemy>();
        if(currentEnemy.hp.currentValue<=currentEnemy.maxHp/2)
        {
            currentAction = Below50Action;
            return;
        }
        if(currentEnemy.hp.currentValue<=currentEnemy.maxHp/5)
        {
            currentAction = Below20Action;
            return;
        }
        var randomIndex = Random.Range(0, enemyActionSO.actions.Count);
        currentAction = enemyActionSO.actions[randomIndex];
    }

    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
        }
    }

    public virtual void Skill()
    {
        // animator.SetTrigger("skill");
        // currentAction.effect.Execute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }

    public virtual void Attack()
    {
        // animator.SetTrigger("attack");
        // currentAction.effect.Execute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime%1.0f>0.6
                                        && !animator.IsInTransition(0) &&
                                        animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));

        if(actionName=="attack")
        {
            currentAction.effect.Execute(this, player);
        }
        else if(actionName=="skill")
        {
            currentAction.effect.Execute(this, this);
        }
    }
}
