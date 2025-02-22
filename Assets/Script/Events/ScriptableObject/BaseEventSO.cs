using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//模板类, 用于定义事件的类型
public class BaseEventSO<T> : ScriptableObject
{
    [TextArea] //添加事件描述 方便检查用
    public string description;
    public UnityAction<T> OnEventRaised;

    public string lastSender; //查看最后广播的对象
    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}
