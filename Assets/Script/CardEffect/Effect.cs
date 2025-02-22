using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int value;
    public EffectTargetType targetType;

    public abstract void Execute(CharacterBase from, CharacterBase target);
}
