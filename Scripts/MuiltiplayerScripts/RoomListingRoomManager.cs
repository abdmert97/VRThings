﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;

public class RoomListingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string waitingRoomSceneName;
    [SerializeField] private GameObject lobbySelectionPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private Transform playersScroller;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Text roomNameText;
    [SerializeField] private SceneIndex sceneIndex;
    public RoomListingLobbyManager roomListingLobbyManager;
   
    private void Awake()
    {
        sceneIndex = Instantiate(sceneIndex);

    }
    void ClearPlayerList()
    {
        foreach (Transform player in playersScroller.transform)
        {
            Destroy(player.gameObject);
        }
    }

    void ListPlayers()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = Instantiate(playerPrefab, playersScroller);
            Text playerNameText = playerObject.transform.GetChild(0).GetComponent<Text>();
            playerNameText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(true);
        lobbySelectionPanel.SetActive(false);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        ClearPlayerList();
        ListPlayers();
        roomListingLobbyManager.UpdateGameNameText("Game: " + PhotonNetwork.CurrentRoom.CustomProperties["gameName"]);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerList();
        ListPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerList();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            SceneManager.LoadScene(sceneIndex.FindSceneIndex(waitingRoomSceneName));
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void OnClickLeave()
    {
        lobbySelectionPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        roomListingLobbyManager.ClearLobbyList();
        StartCoroutine(rejoinLobby());
    }
    
   
}
