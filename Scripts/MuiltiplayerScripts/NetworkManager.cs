using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public RoomListingLobbyManager lobbyManager;
    // Start is called before the first frame update
    void Start()
    { 
        Screen.fullScreen = !Screen.fullScreen;
        if(PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server at region: " + PhotonNetwork.CloudRegion);
        if (MultiplayerSetup.Instance.GetIsTestInstance())
        {
            PhotonNetwork.JoinOrCreateRoom("testroom#001", new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = (byte)(MultiplayerSetup.Instance.GetRoomSize())}, PhotonNetwork.CurrentLobby);
        }
        if (MultiplayerSetup.Instance.GetIsMasterClient())
        {
            lobbyManager.CreateRoom(MultiplayerSetup.Instance.GetRoomName(), MultiplayerSetup.Instance.GetRoomSize());
        }

        
    }
}
