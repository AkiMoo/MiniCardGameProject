using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreakManaLimitEffect", menuName = "Good Effect/BreakManaLimitEffect")]
public class BreakManaLimitEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        Player player = from as Player;
        if(targetType == EffectTargetType.Self)
        {
            player.BreakManaLimit(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            return;
        }
    }
}
