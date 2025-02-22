using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class GoodManager : MonoBehaviour
{
    
    public List<GoodDataSO> goodDataList;

    [Header("卡牌库")]
    //游戏可以翻寻的物品库
    public GoodLibrarySO goodsLibrary;

    //全局变量，用于随机性的前后记录
    private int previousIndex = 0;

    //放到Awake就只执行一次，通关后会继承，否则要另写函数
    private void Awake()
    {
        InitializeGoodDataList();
    }    

    private void OnDisable()
    {
        goodsLibrary.goodList.Clear();
    }

    private void InitializeGoodDataList()
    {
        Addressables.LoadAssetsAsync<GoodDataSO>("GoodData",null).Completed += OnGoodDataLoaded;
    }

    private void OnGoodDataLoaded(AsyncOperationHandle<IList<GoodDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            goodDataList = new List<GoodDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("加载卡牌数据失败！");
        }
    }
    public void ExecuteGoodEffect(GoodDataSO goodData, CharacterBase from, CharacterBase target)
    {
        foreach (var effect in goodData.effects)
        {
            effect.Execute(from, target);
        }
        //Debug.Log("Card ExecuteEffect");
    }

    public GoodDataSO GetGoodData()
    {
        //随机抽卡，这里已经不算随机，他这里两张牌抽得必定不一样
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, goodDataList.Count);
        } while (randomIndex == previousIndex);
        previousIndex = randomIndex;
        return goodDataList[randomIndex];
    }    
}
