using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] private Text roomNameText;
    [SerializeField] private Text playerCountText;
    [SerializeField] private Text gameNameText;
    private string roomName;
    private string gameName;
    private int playerCount;
    private int currentPlayerCount;

    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void UpdateRoomDetails(string newRoomName, string newGameName, int newCurrentPlayerCount, int newPlayerCount)
    {
        roomName = newRoomName;
        gameName = newGameName;
        currentPlayerCount = newCurrentPlayerCount;
        playerCount = newPlayerCount;
        roomNameText.text = roomName;
        gameNameText.text = gameName;
        playerCountText.text = "(" + currentPlayerCount + "/" + playerCount + ")";
    }
}
