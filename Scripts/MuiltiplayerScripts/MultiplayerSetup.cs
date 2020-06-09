using System;
using CommonScripts;
using Photon.Pun;
using UnityEngine;

public class MultiplayerSetup : MonoBehaviour
{
    private bool _isMultiplayerGame = false;

    public bool IsMultiplayerGame
    {
        get => _isMultiplayerGame;
        set => _isMultiplayerGame = value;
    }

    private bool _isMasterClient;
    private int _selectedEnvironment = 0;
    private string _selectedGame;
    private string _roomName;
    private int _roomSize;
    private bool _isTestInstance = false;

    public static MultiplayerSetup Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Instance);
        }
    }


    public void SetIsMasterClient(bool isMasterClient)
    {
        this._isMasterClient = isMasterClient;
    }
    
    public void SetIsTestInstance(bool isTestInstance)
    {
        this._isTestInstance = isTestInstance;
    }

    public void SetSelectedEnvironment(int selectedEnvironment)
    {
        this._selectedEnvironment = selectedEnvironment;
    }

    public int GetSelectedEnvironment()
    {
        return this._selectedEnvironment;
    }
    public void SetSelectedGame(string selectedGame)
    {
        this._selectedGame = selectedGame;
    }

    public void SetRoomName(string roomName)
    {
        this._roomName = roomName;
    }

    public string GetRoomName()
    {
        return this._roomName;
    }

    public void SetRoomSize(string roomSize)
    {
        this._roomSize = Int32.Parse(roomSize);
    }

    public int GetRoomSize()
    {
        return this._roomSize;
    }

    public string GetSelectedGame()
    {
        return this._selectedGame;
    }

    public bool GetIsMasterClient()
    {
        return this._isMasterClient;
    }
    public bool GetIsTestInstance()
    {
        return this._isTestInstance;
    }

    public void DisconnectFromServer()
    {
        if(IsMultiplayerGame && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Destroy(this.gameObject);
        }
    }
}
