using System;

[Flags]

public enum RoomType
{
    //选用幂次+[Flags]标记就可以进行Enum多选，因为是使用二进制实现的或
    Normal = 1,
    Elite = 2,
    Boss = 4,
    Shop = 8,
    Treasure = 16,
    RestRoom = 32
}

public enum RoomState
{
    isLocked,
    visited,
    isAvailiable
}

public enum EffectTargetType
{
    Self,
    Target,
    All,
}

public enum CardType
{
    Attack,
    Defence,
    Ability,
    Heal,
    Event,
}