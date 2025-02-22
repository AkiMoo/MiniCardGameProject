using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomData")]

public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    
    public RoomType roomType;
    public AssetReference SceneToLoad;
}