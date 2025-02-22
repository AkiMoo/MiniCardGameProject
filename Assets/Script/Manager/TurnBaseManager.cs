using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public GameObject playerObj;
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool isBattleEnd = true;
    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("时间广播")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Update(){
        if(isBattleEnd){
            return;
        }
        if(isEnemyTurn){
            timeCounter += Time.deltaTime;
            if(timeCounter >= enemyTurnDuration){
                timeCounter = 0;
                EnemyTurnEnd();
                //通知回合转换
                isPlayerTurn = true;
            }
        }
        if(isPlayerTurn){
            timeCounter += Time.deltaTime;
            if(timeCounter >= playerTurnDuration){
                timeCounter = 0;
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }
    [ContextMenu("Game Start")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        isBattleEnd = false;
        timeCounter = 0;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBegin.RaiseEvent(null,this);
    }
    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null,this);
    }
    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null,this);
    }

    public void OnLoadRoomEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.Normal:
            case RoomType.Elite:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                //可以在这里加入一些适合场景的动画
                break;
        }
    }

    //传入参数不使用，只是为了方便找到
    public void StopTurnBaseSystem(object obj)
    {
        isBattleEnd = true;
        isPlayerTurn = false;
        playerObj.SetActive(false);
    }

    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
        //isBattleEnd = false;
    }
}
