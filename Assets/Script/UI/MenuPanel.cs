using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, exitGameButton;

    public ObjectEventSO newGameEvent, loadHelpEvent;
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        exitGameButton = rootElement.Q<Button>("ExitGameButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        exitGameButton.clicked += OnExitGameButtonClicked;
    }
    //调用unity内置的关闭方式
    private void OnExitGameButtonClicked() => Application.Quit();

    private void OnNewGameButtonClicked()
    {
        //newGameEvent.RaiseEvent(null, this);
        loadHelpEvent.RaiseEvent(null, this);
    }
}
