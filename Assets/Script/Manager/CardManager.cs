using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;

    //所有卡牌的列表
    public List<CardDataSO> cardDataList;
    public List<CardDataSO> treasureCardLibrary;

    [Header("卡牌库")]
    //新游戏开始时的库
    public CardLibrarySO newGameCardLibrary;
    //当前玩家拥有的卡牌
    public CardLibrarySO currentCardLibrary;

    //全局变量，用于随机抽卡记录
    private int previousIndex = 0;

    //放到Awake就只执行一次，通关后会继承，否则要另写函数
    private void Awake()
    {
        InitializeCardDataList();

        foreach (var item in newGameCardLibrary.cardList)
        {
            currentCardLibrary.cardList.Add(item);
        }
    }    

    private void OnDisable()
    {
        currentCardLibrary.cardList.Clear();
    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData",null).Completed += OnCardDataLoaded;
        Addressables.LoadAssetsAsync<CardDataSO>("TreasureData",null).Completed += OnTreasureCardLoaded;
    }


    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("加载卡牌数据失败！");
        }
    }
    private void OnTreasureCardLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            treasureCardLibrary = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("加载宝藏卡牌数据失败！");
        }
    }

    //抽卡时调用的函数获得卡牌GameObject
    public GameObject GetCardObject()
    {
        //之前没有显示是因为这里scale变成0了，要在carddeck使用dotween
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
        //return poolTool.GetObjectFromPool();
    }

    public void DiscardCard(GameObject cardObj)
    {
        //将对象放回池子里
        poolTool.ReleaseObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardData()
    {
        //随机抽卡，这里已经不算随机，他这里两张牌抽得必定不一样
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        } while (randomIndex == previousIndex);
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public CardDataSO GetTreasureCardData()
    {
        //随机抽卡，这里已经不算随机，他这里两张牌抽得必定不一样
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, treasureCardLibrary.Count);
        } while (randomIndex == previousIndex);
        previousIndex = randomIndex;
        return treasureCardLibrary[randomIndex];
    }
    //解锁卡牌的新效果, 直接替换掉原有的？
    public void UnlockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1
        };
        currentCardLibrary.cardList.Add(newCard);
        // if(currentCardLibrary.cardList.Contains(newCard))
        // {
        //     //已经存在的卡
        //     var target = currentCardLibrary.cardList.Find(t => t.cardData == newCardData);
        //     target.amount++;
        //     Debug.Log(target.cardData.name+" "+target.amount);
        // }
        // else
        // {
        //     currentCardLibrary.cardList.Add(newCard);
        // }
    }

}
