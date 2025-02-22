using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject gameFinishPanel;

    public GameObject pickCardPanel;

    public GameObject restRoomPanel;
    public GameObject shopPanel;
    public GameObject treasurePanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = data as Room;
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.Normal:
            case RoomType.Elite:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                shopPanel.SetActive(true);
                break;
            case RoomType.Treasure:
                treasurePanel.SetActive(true);
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);
                break;
        }

    }
    public void HideAllPanels()
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameFinishPanel.SetActive(false);
        restRoomPanel.SetActive(false);
        shopPanel.SetActive(false);
        treasurePanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnGameFinishEvent()
    {
        gameplayPanel.SetActive(false);
        gameFinishPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
