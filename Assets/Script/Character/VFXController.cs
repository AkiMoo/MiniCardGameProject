using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject buff, debuff;
    private float timeCounter;
    private void Update()
    {
        if(buff.activeInHierarchy){
            timeCounter += Time.deltaTime;
            if(timeCounter >= 1.2f){
                timeCounter = 0f;
                buff.SetActive(false);
            }
        }

        if(debuff.activeInHierarchy){
            timeCounter += Time.deltaTime;
            if(timeCounter >= 1.2f){
                timeCounter = 0f;
                debuff.SetActive(false);
            }
        }
    }
}
