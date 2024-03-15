using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    private UIDocument _uIDocument;
    private VisualElement _rootElement;

    private Button _hostGameButton;
    private Button _joiGameButton;
    private Button _lobbiesButton;

    private TextField _joinCodeField;

    private Button _searchGameButton;

    [SerializeField] private GameObject lobbyGameObjectUI;
    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;
        _hostGameButton = _rootElement.Q<Button>("HostGameButton");
        _joiGameButton = _rootElement.Q<Button>("ClientJoinButton");
        _joinCodeField = _rootElement.Q<TextField>("JoinCodeField");
        _lobbiesButton = _rootElement.Q<Button>("LobbiesButton");
        _searchGameButton = _rootElement.Q<Button>("SearchGameButton");

        _hostGameButton.clicked += HostGameAsync;
        _joiGameButton.clicked += JoinGameAsync;
        _lobbiesButton.clicked += OpenLobbiesView;
        _searchGameButton.clicked += MatchMakingSearch;
    }

    private void MatchMakingSearch()
    {
        ClientSingleton.GetInstance().StartMatchMaking();
    }

    private void OnDisable()
    {
        _hostGameButton.clicked -= HostGameAsync;
        _joiGameButton.clicked -= JoinGameAsync;
        _lobbiesButton.clicked -= OpenLobbiesView;
        _searchGameButton.clicked -= MatchMakingSearch;
    }


    private async void JoinGameAsync()
    {
        await ClientSingleton.GetInstance().StartClientAsync(_joinCodeField.text);
    }

    private async void HostGameAsync()
    {
        await HostSingleton.GetInstance().StartHost();
    }

    private void OpenLobbiesView()
    {
        lobbyGameObjectUI.SetActive(true);
        gameObject.SetActive(false);
    }




}
