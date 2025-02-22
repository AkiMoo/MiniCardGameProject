using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CardLayoutManager : MonoBehaviour
{
    //判断排布方式
    public bool isHorizontal;
    //摄像机宽度距离设定，如摄像机大小设为6，分辨率为1920*1080的话，展示的物体范围为（-10.7，-6）~ （10.7,9）
    //让手牌范围为（-3.5~3.5）
    public float maxWidth = 7f;
    //卡牌之间间隔大小
    public float cardSpace = 3f;
    [Header("弧形参数")]
    //这个弧度角计算方式为：arcsin（宽/2/半径）*2
    public float maxAngle = 24f;
    //卡牌之间的角度
    public float angleBetweenCard = 7f;
    //卡牌的旋转半径
    public float radius = 17f;
    //定义卡牌展示中心点
    public Vector3 centerpoint;

    //序列化域，用于看到具体的数值

    [SerializeField]
    private List<Vector3> cardPositions = new List<Vector3>();
    private List<Quaternion> cardRotations = new List<Quaternion>();

    private void Awake(){
        centerpoint = isHorizontal? Vector3.up * -4f : Vector3.up * -21f;
    }

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculateCardPositions(totalCards, isHorizontal);
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }
    
    private void CalculateCardPositions(int numberOfCards, bool isHorizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();

        if(isHorizontal)
        {
            float currentWidth = cardSpace * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            //计算卡牌之间的间距
            float currentSpace = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;
            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - totalWidth / 2 + currentSpace * i;
                var pos = new Vector3(xPos, centerpoint.y, 0);
                var rotation = Quaternion.identity;
                //记录位置和旋转
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float totalAngle = (numberOfCards - 1) * angleBetweenCard;
            totalAngle = Mathf.Min(totalAngle, maxAngle);
            float currentAngleBetweenCards;
            if(numberOfCards <= 1)
            {
                currentAngleBetweenCards = 0;
            }
            else{
                currentAngleBetweenCards = totalAngle / (numberOfCards - 1);
            }
            float cardAngle = totalAngle / 2;
            for (int i = 0; i < numberOfCards; i++)
            {
                float angle = cardAngle - currentAngleBetweenCards * i;
                // 算其他卡牌的角度
                var pos = FanCardPosition(angle);
                var rotation = Quaternion.Euler(0, 0, angle);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerpoint.x - Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
            centerpoint.y + Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            0
        );
    }
}
