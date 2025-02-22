using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    // Start is called before the first frame update
    public string cardName;
    public Sprite cardImage;
    public CardType cardType;

    public int cardCost;

    [TextArea]
    public string cardDescription;

    //因为一张牌可能有多个效果，需要List
    public List<Effect> effects;
    
}
