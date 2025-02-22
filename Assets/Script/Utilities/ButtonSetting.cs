using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetting : MonoBehaviour
{
    public ObjectEventSO newGameEvent;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        Debug.Log("NowStartGame");
        newGameEvent.RaiseEvent(null, this);
    }
}
