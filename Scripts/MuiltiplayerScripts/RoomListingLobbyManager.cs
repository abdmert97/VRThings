using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomListingLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject lobbyJoinButton;
    [SerializeField] private GameObject lobbyLoadingButton;
    [SerializeField] private GameObject lobbySelectionPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private InputField playerNameInput;

    private string lobbyName;

    private int playerCount;

    private List<RoomInfo> lobbyList;
    [SerializeField] private Transform lobbyScroller;
    [SerializeField] private GameObject lobbyPrefab;

    public Text gameNameTextLoginPanel;
    public Text gameNameTextLobbySelectionPanel;
    public Text gameNameTextLobbyPanel;

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateGameNameText("Game: " + MultiplayerSetup.Instance.GetSelectedGame());
        }
        else
        {
            UpdateGameNameText("Multiplayer Lobby");
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyJoinButton.SetActive(true);
        lobbyLoadingButton.SetActive(false);
        lobbyList = new List<RoomInfo>();

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            if (PlayerPrefs.GetString("PlayerName") == "")
            {
                PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
        }

        playerNameInput.text = PhotonNetwork.NickName;
    }

    public void UpdatePlayerName(string newPlayerName)
    {
        PhotonNetwork.NickName = newPlayerName;
        PlayerPrefs.SetString("PlayerName", newPlayerName);
        Debug.Log("Player name updated" + newPlayerName);
    }

    public void OnClickJoin()
    {
        loginPanel.SetActive(false);
        lobbySelectionPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    private static Predicate<RoomInfo> ByName(string roomName)
    {
        return room => room.Name == roomName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            var roomInPanel = lobbyList.Find(ByName(room.Name));
            if (roomInPanel != null)
            {
                RemoveRoomFromPanel(room.Name);
            }

            if (room.PlayerCount > 0)
            {
                lobbyList.Add(room);
                AddRoomToPanel(room);
            }
        }
    }

    private void AddRoomToPanel(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject lobbyObject = Instantiate(lobbyPrefab, lobbyScroller);
            RoomEntry roomEntry = lobbyObject.GetComponent<RoomEntry>();
            roomEntry.UpdateRoomDetails(room.Name, room.CustomProperties["gameName"] as string, room.PlayerCount, room.MaxPlayers);
        }
    }

    private void RemoveRoomFromPanel(string roomName)
    {
        int roomIndex = lobbyList.FindIndex(ByName(roomName));
        Destroy(lobbyScroller.GetChild(roomIndex).gameObject);
        lobbyList.RemoveAt(roomIndex);
    }

    public void OnRoomNameChanged(string newRoomName)
    {
        lobbyName = newRoomName;
        Debug.Log("Room name changed" + lobbyName);
    }

    public void OnPlayerCountChanged(string newPlayerCount)
    {
        playerCount = Int32.Parse(newPlayerCount);
        Debug.Log("Player count changed " + playerCount);
    }

    public void CreateRoom()
    {
        if (lobbyName == null || playerCount == 0)
        {
            Debug.Log("Please enter correct lobby specs");
            return;
        }

        Debug.Log("Room created, " + lobbyName + " - " + playerCount);
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = (byte) playerCount};
        roomOptions.CustomRoomPropertiesForLobby = new string[1] { "gameName" };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{"gameName", MultiplayerSetup.Instance.GetSelectedGame()}};
        roomOptions.CustomRoomProperties["gameName"] = MultiplayerSetup.Instance.GetSelectedGame();
        Debug.Log(roomOptions.CustomRoomProperties["gameName"]);
        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
    }

    public void CreateRoom(string roomName, int playerCount)
    {
        Debug.Log("Room created, " + roomName + " - " + playerCount);
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = (byte) playerCount};
        roomOptions.CustomRoomPropertiesForLobby = new string[1] { "gameName" };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{"gameName", MultiplayerSetup.Instance.GetSelectedGame()}};
        roomOptions.CustomRoomProperties["gameName"] = MultiplayerSetup.Instance.GetSelectedGame();
        Debug.Log(roomOptions.CustomRoomProperties["gameName"]);
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.CurrentRoom.CustomProperties.Add("game", MultiplayerSetup.Instance.GetSelectedGame());
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed!");
    }

    public void Cancel()
    {
        loginPanel.SetActive(true);
        lobbySelectionPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateGameNameText(string text)
    {
        gameNameTextLoginPanel.text = gameNameTextLobbyPanel.text = gameNameTextLobbySelectionPanel.text = text;
    }

    public void ClearLobbyList()
    {
        lobbyList.Clear();
        foreach (Transform child in lobbyScroller) {
            Destroy(child.gameObject);
        }
    }
}