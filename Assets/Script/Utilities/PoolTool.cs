using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject cardPrefab;
    public ObjectPool<GameObject> cardPool;
    void Awake()
    {
        cardPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(cardPrefab,transform),
            actionOnGet: (card) => card.SetActive(true),
            //回收
            actionOnRelease: (card) => card.SetActive(false),
            //销毁
            actionOnDestroy: (card) => Destroy(card),

            //是否需要检查对象
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        PreFillPool(7);
    }
    //预先填充变量池，count是数量
    private void PreFillPool(int count){
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = cardPool.Get();
        }
        //这里不是很懂，get完active了但是又release了，感觉有点奇怪
        //这里其实是预先填充，用数组保存取到的数据，再将他们放回池子
        foreach (var item in preFillArray)
        {
            cardPool.Release(item);
        }
    }
    //这里是预留给其他要get和release的地方调用的函数接口
    public GameObject GetObjectFromPool()
    {
        return cardPool.Get();
    }

    public void ReleaseObjectToPool(GameObject obj)
    {
        cardPool.Release(obj);
    }
}
