using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
