using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/Healffect")]

public class HealEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            from.UpdateHealth(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.UpdateHealth(value);
        }
    }
}
