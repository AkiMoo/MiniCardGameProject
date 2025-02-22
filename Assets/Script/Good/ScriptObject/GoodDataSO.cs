using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodDataSO", menuName = "Good/GoodDataSO")]
public class GoodDataSO : ScriptableObject
{
    // Start is called before the first frame update
    public string goodName;
    public Sprite goodImage;
    public int price;

    [TextArea]
    public string goodDescription;

    //因为一张牌可能有多个效果，需要List
    public List<Effect> effects;
    
}
