using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStrengthEffect", menuName = "Good Effect/BaseStrengthEffect")]
public class BaseStrengthEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            from.UpdateBaseStrength(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.UpdateBaseStrength(value);
        }
    }
}
