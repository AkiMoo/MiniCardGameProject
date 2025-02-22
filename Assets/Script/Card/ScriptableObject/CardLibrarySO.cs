using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardLibrarySO", menuName = "Card/CardLibrarySO")]
public class CardLibrarySO : ScriptableObject
{
    public List<CardLibraryEntry> cardList;

}
[System.Serializable]
public struct CardLibraryEntry
{
    public CardDataSO cardData;
    //amount是这张卡的数量
    public int amount;
}