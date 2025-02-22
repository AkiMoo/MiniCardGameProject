using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpPanel : MonoBehaviour
{
    private VisualElement rootElement;
    public GameObject helpPanel;
    private Button backButton;
    private void OnEnable()
    {
        rootElement  = GetComponent<UIDocument>().rootVisualElement;
        backButton = rootElement.Q<Button>("Back");
        backButton.clicked += OnBackButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        helpPanel.SetActive(false);
    }
}
