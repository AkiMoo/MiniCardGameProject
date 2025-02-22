using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private UnityEngine.UIElements.Button backToStartButton;
    public ObjectEventSO loadMenuEvent;
    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Button>("BackToStart").clicked += OnBackToStartButtonClicked;
    }
    private void OnBackToStartButtonClicked()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
