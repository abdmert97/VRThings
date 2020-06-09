﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

 public class Testroom : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
 {
        public AppSettings appSettings;
        private LoadBalancingClient lbc;

        private ConnectionHandler ch;
        public Text StateUiText;

        public void Start()
        {
            this.lbc = new LoadBalancingClient();
            this.lbc.AddCallbackTarget(this);
            this.lbc.SerializationProtocol = SerializationProtocol.GpBinaryV16;
            //this.lbc.LoadBalancingPeer.SocketImplementationConfig[ConnectionProtocol.Udp] = typeof(SocketUdpSrc);
            
            if (!this.lbc.ConnectUsingSettings(appSettings))
            {
                Debug.LogError("Error while connecting");
            }

            this.ch = this.gameObject.GetComponent<ConnectionHandler>();
            if (this.ch != null)
            {
                this.ch.Client = this.lbc;
                this.ch.StartFallbackSendAckThread();
            }
        }

        public void Update()
        {
            LoadBalancingClient client = this.lbc;
            if (client != null)
            {
                client.Service();


                Text uiText = this.StateUiText;
                string state = client.State.ToString();
                if (uiText != null && !uiText.text.Equals(state))
                {
                    uiText.text = "State: " + state;
                }
            }
        }


        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            EnterRoomParams roomParams = new EnterRoomParams();
            roomParams.RoomName = "vr";
            roomParams.CreateIfNotExists = true;
            roomParams.RoomOptions = new RoomOptions();
            roomParams.RoomOptions.MaxPlayers = 2;
            this.lbc.OpJoinRoom(roomParams);    // joins any open room (no filter)
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnJoinedLobby()
        {
        }

        public void OnLeftLobby()
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            this.lbc.OpCreateRoom(new EnterRoomParams());
        }

        public void OnLeftRoom()
        {
        }


        /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
        /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
            this.lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
        }
    }