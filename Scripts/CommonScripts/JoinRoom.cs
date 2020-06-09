﻿using System;
using System.Collections;
using System.Collections.Generic;

 using UnityEngine;
using Photon;
 using Photon.Pun;
 using Photon.Realtime;


 public class JoinRoom : MonoBehaviourPun
{

    public string roomName;
    public bool AutoConnect = true;

    public byte Version = 1;

  
    private bool ConnectInUpdate = true;




    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.IsConnected)
        {
            Debug.Log("VrThings started. Joining a room");
            ConnectInUpdate = false;
            
           
        }
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("Connecting a random room");
        if(!roomName.Equals(""))
        {
           
            PhotonNetwork.JoinRoom(roomName);
          
          
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public virtual void OnPhotonCreateGameFailed()
    {
        ConnectInUpdate = true;
        Debug.Log("Create room failed");
    }
    public virtual void OnJoinedLobby()
    {
        if(!roomName.Equals(""))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public virtual void OnPhotonJoinRoomFailed()
    {
        PhotonNetwork.CreateRoom(roomName);
    }
    public virtual void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(roomName, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        ConnectInUpdate = true;
        Debug.LogError("Failed to connect due to: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("You joined a room");
    }
}
