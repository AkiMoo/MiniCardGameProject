using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList = new();

    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;
    public IntEventSO plusMoneyEvent;
    public ObjectEventSO gameFinishEvent;

    //更新房间的事件监听函数,另外，如果需要监听事件的话，这里传入的参数要是object类型
    public void UpdateMapLayout(object value)
    {
        Debug.Log("更新地图");
        var roomVector = (Vector2Int)value;
        if(mapLayout.mapRoomDataList.Count == 0)
        {
            return;
        }
        var currentRoom = mapLayout.mapRoomDataList.Find(room => room.column == roomVector.x && room.row == roomVector.y);
        currentRoom.roomState = RoomState.visited;
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(room => room.column == currentRoom.column);
        foreach(var room in sameColumnRooms){
            if(room.row == roomVector.y)
            {
                continue;
            }
            room.roomState = RoomState.isLocked;
        }
        foreach(var item in currentRoom.linkTo)
        {
            var linkRoom = mapLayout.mapRoomDataList.Find(room => room.column == item.x && room.row == item.y);
            linkRoom.roomState = RoomState.isAvailiable;
            Debug.Log("房间");
        }
    }
    public void OnLoadRoomEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }
    public void OnCharacterDeadEvent(object character)
    {
        if(character is Player)
        {
            //发出失败的通知
            aliveEnemyList.Clear();
            StartCoroutine(EventDelayAction(gameOverEvent));
            return;
            //gameOverEvent.RaiseEvent(null,this);
        }
        
        //如果只是返回主场景则不做区分
        if(character is Boss)
        {
            StartCoroutine(EventDelayAction(gameFinishEvent));
            aliveEnemyList.Remove((Boss)character);
        }
        else if(character is Enemy)
        {
            
            aliveEnemyList.Remove((Enemy)character);
            if(aliveEnemyList.Count == 0)
            {
                Debug.Log("死亡的object是:" + character.GetType());
                //发出胜利的通知
                StartCoroutine(EventDelayAction(gameWinEvent));
                plusMoneyEvent.RaiseEvent(10,this);
                //gameWinEvent.RaiseEvent(null,this);
            }
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        
        eventSO.RaiseEvent(null,this);
    }
    //新的游戏开始，要初始化这个地图

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.LinePositionList.Clear();
    }
}
