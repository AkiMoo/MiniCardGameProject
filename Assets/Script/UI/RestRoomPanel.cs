using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootVisualElement;
    private Button restButton;
    private Button backToButton;

    public Effect restEffect;
    public ObjectEventSO loadMapEvent;

    private CharacterBase player;

    private void OnEnable()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootVisualElement.Q<Button>("RestButton");
        backToButton = rootVisualElement.Q<Button>("BackToMap");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restButton.clicked += OnRestButtonClicked;
        backToButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnRestButtonClicked()
    {   
        //restButton.style.display = DisplayStyle.None;
        //为什么加了display能点两次？
        restEffect.Execute(player, null);
        restButton.SetEnabled(false);
        restButton.style.color = Color.gray;
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null,this);
    }
}
