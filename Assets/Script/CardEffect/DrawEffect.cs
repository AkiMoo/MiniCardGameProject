using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawEffect", menuName = "Card Effect/DrawEffect")]
public class DrawEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value, this);
    }
}
