
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference help;
    public AssetReference menu;

    private Vector2Int currentRoomVector;
    private Room currentRoom;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO UpdateRoomEvent;
    
    private void Awake()
    {
        currentRoomVector = Vector2Int.one * - 1;
        LoadMenu();
    }
    
    public async void OnLoadRoomEvent(object data)
    {
        if(data is Room)
        {
            currentRoom = (Room)data;
            var currentData = currentRoom.roomData;
            Debug.Log("Now Room is :" + currentRoom.colunm + " " + currentRoom.row);
            currentRoomVector = new(currentRoom.colunm, currentRoom.row);
            Debug.Log(currentData.roomType);
            //获取当前场景
            currentScene = currentData.SceneToLoad;
        }
        //加载房间
        await UnLoadSceneTask();
        await LoadSceneTask();
        afterRoomLoadedEvent.RaiseEvent(currentRoom,this);
    }
    //异步操作加载场景
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        //这个Task是上面方法内置的吗？
        await s.Task;
        if(s.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Happen!");
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnLoadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
    //返回地图
    public async void LoadMap()
    {
        await UnLoadSceneTask();
        // 这里目前有问题，加了if判断之后进不去
        Debug.Log("Now Room is :" + currentRoomVector);
        if(currentRoomVector!= Vector2.one*-1)
        {
            Debug.Log("Come here");
            UpdateRoomEvent.RaiseEvent(currentRoomVector, this);
        }
        //UpdateRoomEvent.RaiseEvent(currentRoomVector, this);
        currentScene = map;
        await LoadSceneTask();
    }
    public async void LoadMenu()
    {
        if(currentScene!=null)
        {
            await UnLoadSceneTask();
        }
        
        currentScene = menu;
        await LoadSceneTask();
    }
    public async void LoadHelp()
    {
        if(currentScene!=null)
        {
            await UnLoadSceneTask();
        }
        currentScene = help;
        await LoadSceneTask();
    }
}
