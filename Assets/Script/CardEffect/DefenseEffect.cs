using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            from.UpdateDefense(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.UpdateDefense(value);
        }
    }
}
