using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeEffect", menuName = "Good Effect/LifeEffect")]
public class LifeEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            from.UpdateMaxHp(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.UpdateMaxHp(value);
        }
    }
}
