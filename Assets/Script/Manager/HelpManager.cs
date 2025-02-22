using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public TMP_Text explainText;
    public TMP_Text playingText;
    public TMP_Text coinText;
    public TMP_Text costText;
    public TMP_Text manaText, deckText, discardText, skillText, periodText;
    public List<string> textToPrint = new List<string>();
    public List<TMP_Text> Text = new List<TMP_Text>(); 

    [Header("Event")]
    public ObjectEventSO newGameEvent;
    //文段输出计数器
    IEnumerator TypeText(TMP_Text tMP_Text, TMP_Text tMP_Text2, string str, string str2, float interval)
    {
        int i = 0;
        while (i<str.Length)
        {
            tMP_Text.text = str.Substring(0, i++);
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(0.5f);
        i = 0;
        while (i<str2.Length)
        {
            tMP_Text2.text = str2.Substring(0, i++);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator TypeTextRest(List<TMP_Text> tMP_Text, List<string> str, float interval)
    {
        //等前面的放完
        yield return new WaitForSeconds(6.5f);
        for(int j = 0; j<tMP_Text.Count;j++){
            int i = 0;
            while (i<str[j].Length)
            {
                tMP_Text[j].text = str[j].Substring(0, i++);
                yield return new WaitForSeconds(interval);
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    public void NowStartGame()
    {
        Debug.Log("NowStartGame");
        newGameEvent.RaiseEvent(null, this);
    }
    private void Start()
    {
        textToPrint.Add("卡牌左上角的圆槽数字代表这张卡牌需要的费用点数! ");
        textToPrint.Add("金币, 玩家初始拥有30枚, 战胜敌人可获得10枚, 用于在商店中消费!  ");
        textToPrint.Add("能力水晶, 打出卡牌或者使用技能需要消耗指定的点数。每回合开始时回复1点能力值。 ");
        textToPrint.Add("卡组剩余数量。每次轮到玩家回合时抽2张卡。当玩家要抽卡但剩余数量不能满足抽卡数量的时候, 玩家败北。 ");
        textToPrint.Add("玩家本轮次使用的卡牌数量。 (No Special Function) ");
        textToPrint.Add("技能面板，数字代表需要的能力水晶数量。 ");
        textToPrint.Add("注意: 二周目会继承所有已获得的卡牌! ");
        Text.Add(costText);
        Text.Add(coinText);
        Text.Add(manaText);
        Text.Add(deckText);
        Text.Add(discardText);
        Text.Add(skillText);
        Text.Add(periodText);
        StartCoroutine(TypeText(explainText, playingText,
         "    现在,你将作为勇者,讨伐位于地图终点的强大敌人! 在旅途中不断变强吧! \n    下面是勇者手册的内容: ",
         "卡牌打出: 选中并移动卡牌, 并在高于屏幕1/2的位置上释放! \n能力使用: 点击位于屏幕左上方的技能按钮! ",
          0.05f));
        StartCoroutine(TypeTextRest(Text, textToPrint, 0.05f));
    }

}
