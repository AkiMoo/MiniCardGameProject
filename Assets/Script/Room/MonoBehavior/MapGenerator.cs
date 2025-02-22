using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;
    private float colunmWidth;
    //写成public才能在aspect界面设置
    public float border;
    //生成起始点的变量
    private Vector3 generatePoint;

    //此处需要初始化
    private List<Room> rooms  = new();
    private List<LineRenderer> lines = new();
    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();
    
    public void Awake()
    {
        //世界坐标的长和宽
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        colunmWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);

        foreach (var item in roomDataList)
        {
            roomDataDict.Add(item.roomType, item);
        }
    }
    
    private void OnEnable()
    {
        //Debug.Log(mapLayout.mapRoomDataList.Count);
        if(mapLayout.mapRoomDataList.Count>0)
        {
            LoadMap();
            //Debug.Log("LoadMap");
        }
        else{
            CreateMap();
        }
    }
    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> prevRooms = new();

        for (int colunm = 0; colunm < mapConfig.roomBlueprints.Count; colunm++)
        {
            var blueprint = mapConfig.roomBlueprints[colunm];
            var amount = Random.Range(blueprint.min, blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight/(amount+1);
            generatePoint = new Vector3(-screenWidth/2+border + colunmWidth * colunm, startHeight, 0);
            //向右移动一定位置，再生成
            var newPosition = generatePoint;
            List<Room> currentRooms = new();
            var roomGapY = screenHeight / (amount + 1);
            
            for(int i=0; i< amount; i++)
            {
                newPosition.y = startHeight - i * roomGapY;
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                //生成房间（随机）
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[colunm].roomType);
                //设置起始房间
                room.roomState = RoomState.isAvailiable;
                //Debug.Log($"{room.roomState}");
                if(colunm == 0){
                    //Debug.Log($"进来了");
                    room.roomState = RoomState.isAvailiable;
                }
                else{
                    //Debug.Log($"进来了?");
                    room.roomState = RoomState.isLocked;
                }
                room.SetupRoom(colunm,i,GetRoomData(newType));               
                rooms.Add(room);
                currentRooms.Add(room);
            }
            //连接房间
            if (prevRooms.Count > 0)
            {
                //创建连线
                createConnections(prevRooms, currentRooms);
            }
            prevRooms = currentRooms;
        }
        SaveMap();
    }
    private void createConnections(List<Room> prevRooms, List<Room> currentRooms)
    {
        HashSet<Room> connected = new();
        foreach(var room in prevRooms)
        {
            var targetRoom = ConnectToRandomRoom(room, currentRooms, false);
            connected.Add(targetRoom);
        }
        foreach(var room in currentRooms)
        {
            if(!connected.Contains(room))
            {
                ConnectToRandomRoom(room, prevRooms, true);
            }
        }
    }
    private Room ConnectToRandomRoom(Room room, List<Room> currentRooms, bool check)
    {
        Room targetRoom;
        targetRoom = currentRooms[Random.Range(0, currentRooms.Count)];
        if(check)
        {
            targetRoom.linkTo.Add(new(room.colunm, room.row));
        }
        else{
            room.linkTo.Add(new(targetRoom.colunm, targetRoom.row));
        }
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        return targetRoom;
    }

    [ContextMenu("ReGenerate Room")]
    public void ReGenernateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach (var item in lines)
        {
            Destroy(item.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }
    
    //让生成的房间随机
    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption = options[Random.Range(0, options.Length)];
        RoomType randomType = (RoomType)System.Enum.Parse(typeof(RoomType), randomOption);
        return randomType;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        //添加现存房间
        for(int i=0; i<rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].colunm,
                row = rooms[i].row,
                roomDataSO = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo
            };
            mapLayout.mapRoomDataList.Add(room);
        }
        mapLayout.LinePositionList = new();
        for(int i=0; i<lines.Count; i++){
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };
            mapLayout.LinePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        //读取现存房间
        for(int i=0; i<mapLayout.mapRoomDataList.Count; i++){
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].column, mapLayout.mapRoomDataList[i].row, mapLayout.mapRoomDataList[i].roomDataSO);
            newRoom.linkTo = mapLayout.mapRoomDataList[i].linkTo;
            rooms.Add(newRoom);
        }

        //读取线的头尾坐标
        for(int i = 0; i<mapLayout.LinePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.LinePositionList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.LinePositionList[i].endPos.ToVector3());
            lines.Add(line);
        }
    }
}
