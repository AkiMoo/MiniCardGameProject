using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于保存地图，房间和连线
[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new();
    public List<LinePosition> LinePositionList = new();
}

[System.Serializable]
public class MapRoomData
{
    public float posX, posY;
    public int column, row;
    public RoomDataSO roomDataSO;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}

[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos, endPos;
}