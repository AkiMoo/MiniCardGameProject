using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPlusEffect", menuName = "Card Effect/ManaPlusEffect")]
public class ManaPlusEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        Player player = from as Player;
        player.PlusMana(value);
    }
}
