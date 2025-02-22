using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        var realDamage = (int)math.round(from.baseStrength*value);
        if(target == null)
            return;
        switch (targetType)
        {
            //分清楚这个全体，是全体敌人，还是地图上全体，根据卡牌效果类型来判断
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(realDamage);
                }
                break;
            case EffectTargetType.Target:
                target.TakeDamage(realDamage);
                //Debug.Log($"造成了{realDamage}点伤害");
                break;
        }
    }
}
