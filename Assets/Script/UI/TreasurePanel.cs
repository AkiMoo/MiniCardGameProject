using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreasurePanel : MonoBehaviour
{
    private VisualElement rootVisualElement;
    private Button TreasureButton;
    private Button backToButton;
    private Button deckCheck;
    public GameObject deckCheckPanel;

    public ObjectEventSO gameWinEvent;
    public ObjectEventSO loadMapEvent;

    private void OnEnable()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        TreasureButton = rootVisualElement.Q<Button>("TreasureButton");
        backToButton = rootVisualElement.Q<Button>("BackToMap");
        deckCheck = rootVisualElement.Q<Button>("DeckCheck");

        deckCheck.clicked += OnDeckCheckClicked;
        TreasureButton.clicked += OnTreasureButtonClicked;
        backToButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnDeckCheckClicked()
    {
        deckCheckPanel.SetActive(true);
    }

    private void OnTreasureButtonClicked()
    {
        gameWinEvent.RaiseEvent(null,this);
        TreasureButton.SetEnabled(false);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null,this);
    }
}
