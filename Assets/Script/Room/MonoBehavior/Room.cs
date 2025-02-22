using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    //房间的坐标，便于标记
    public int colunm;
    public int row;
    //房间有一个图
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo = new();

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        //选择inchildren是因为挂载的图片视为子物体？
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    //内置的鼠标点击方法
    private void OnMouseDown()
    {
        Debug.Log($"Mouse clicked on {roomState}");
        //用this就可以表明是当前函数发出广播
        if(roomState == RoomState.isAvailiable){
            Debug.Log($"Load room {roomData.roomType}");
            loadRoomEvent.RaiseEvent(this, this);
        }
    }

    //外部创建房间之时数据传入
    public void SetupRoom(int colunm, int row, RoomDataSO roomData)
    {
        this.colunm = colunm;
        this.row = row;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;

        spriteRenderer.color = roomState switch
        {
            RoomState.isLocked => Color.gray,
            RoomState.visited => Color.red,
            RoomState.isAvailiable => Color.green,
            _ => throw new System.NotImplementedException(),
        };
    }
}
