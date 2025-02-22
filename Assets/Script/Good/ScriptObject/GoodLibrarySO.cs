using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodLibrarySO", menuName = "Good/GoodLibrarySO")]
public class GoodLibrarySO : ScriptableObject
{
    public List<GoodLibraryEntry> goodList;

}
[System.Serializable]
public struct GoodLibraryEntry
{
    public GoodDataSO goodData;
    //amount是这张卡的数量
    public int amount;
}