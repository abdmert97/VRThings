using System;
using System.Collections;
using System.Collections.Generic;
using CommonScripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private int _currentPlayerCount;
    private int _maxPlayerCount;
    
    [SerializeField] private string lobbySceneName;

    
    [SerializeField] private int minPlayerCountToStartCountdown;
    [SerializeField] private float minPlayerWaitTme;
    [SerializeField] private float maxPlayerWaitTme;
    
    [SerializeField] private Text roomNameDisplay;
    [SerializeField] private Text playerCountDisplay;
    [SerializeField] private Text timerDisplay;
    
    [SerializeField] private SceneIndex sceneIndex;
    
    private bool _minPlayerReached;
    private bool _maxPlayerReached;
    private bool _loadingGame;
    
    private float _startGameTimer;
    private float _minPlayerJoinedTimer;
    private float _maxPlayerJoinedTimer;

    public Text gameNameText;


    private void Awake()
    {
        sceneIndex = Instantiate(sceneIndex);
    }

    void Start()
    {
        //_sceneAssets = Instantiate(_sceneAssets);
        _photonView = GetComponent<PhotonView>();
        roomNameDisplay.text = "Room id: " + PhotonNetwork.CurrentRoom.Name;
        _maxPlayerJoinedTimer = maxPlayerWaitTme;
        _minPlayerJoinedTimer = minPlayerWaitTme;
        _startGameTimer = minPlayerWaitTme;
        UpdatePlayerCount();
        gameNameText.text = PhotonNetwork.CurrentRoom.CustomProperties["game"] + " Game Waiting Room ";
    }

    void UpdatePlayerCount()
    {
        _currentPlayerCount = PhotonNetwork.PlayerList.Length;
        _maxPlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = _currentPlayerCount + "/" + _maxPlayerCount + " players ready";
        if (_currentPlayerCount == _maxPlayerCount)
        {
            _maxPlayerReached = true;
        }
        else if (_currentPlayerCount >= minPlayerCountToStartCountdown)
        {
            _minPlayerReached = true;
        }
        else
        {
            _minPlayerReached = false;
            _maxPlayerReached = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("SynchronizeStartTimer", RpcTarget.Others, _startGameTimer);
        }
    }

    [PunRPC]
    private void SynchronizeStartTimer(float timeIn)
    {
        _startGameTimer = timeIn;
        _minPlayerJoinedTimer = timeIn;
        if (timeIn < _maxPlayerJoinedTimer)
        {
            _maxPlayerJoinedTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    // Update is called once per frame
    void Update()
    {
        WaitForPlayers();
    }

    void WaitForPlayers()
    {
        if (_currentPlayerCount < 1)
        {
            ResetTimer();
        }

        if (_maxPlayerReached)
        {
            _maxPlayerJoinedTimer -= Time.deltaTime;
            _startGameTimer = _maxPlayerJoinedTimer;
        }
        else if (_minPlayerReached)
        {
            _minPlayerJoinedTimer -= Time.deltaTime;
            _startGameTimer = _minPlayerJoinedTimer;
        }

        var formattedTimer = $"{_startGameTimer:00}";
        timerDisplay.text = formattedTimer;

        if (_startGameTimer <= 0f)
        {
            if (_loadingGame)
            {
                return;
            }
            StartGame();
        }
    }

    void ResetTimer()
    {
        _startGameTimer = minPlayerWaitTme;
        _minPlayerJoinedTimer = minPlayerWaitTme;
        _maxPlayerJoinedTimer = maxPlayerWaitTme;
    }

    void StartGame()
    {
        _loadingGame = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            _photonView.RPC("SetEnviroment",RpcTarget.Others,MultiplayerSetup.Instance.GetSelectedEnvironment());
            PhotonNetwork.LoadLevel(sceneIndex.FindSceneIndex(MultiplayerSetup.Instance.GetSelectedGame()));
        }
    }
    [PunRPC]
    public void SetEnviroment(int index)
    {
     EnviromentLoader.Instance.SetSelectedEnviroment(index );
    }
    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneIndex.FindSceneIndex(lobbySceneName));
    }
    
    
}
