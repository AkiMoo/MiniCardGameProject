using UnityEngine;

//用这个来通知其他脚本，变量的值发生了变化
[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    public IntEventSO valueChangeEvent;

    [TextArea]
    [SerializeField] private string description;

    public void SetValue(int value)
    {
        currentValue = value;
        valueChangeEvent?.RaiseEvent(value,this);
    }
}
