using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    public BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if(baseEventSO == null)
        {
            baseEventSO = (BaseEventSO<T>)target;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("订阅数量:"+GetListeners().Count);
        foreach(var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());
        }

    }

    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> Listeners = new();

        if(baseEventSO == null || baseEventSO.OnEventRaised == null)
        {
            return Listeners;
        }
        var subsribers =baseEventSO.OnEventRaised.GetInvocationList();
        foreach(var sub in subsribers)
        {
            var obj = sub.Target as MonoBehaviour;
            if(!Listeners.Contains(obj))
            {
                Listeners.Add(obj);
            }
        }
        return Listeners;
    }
}
