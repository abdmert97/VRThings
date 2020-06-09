using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Checkers;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class MultiplayerObjectCreator : Tools.Singleton<MultiplayerObjectCreator>,IOnEventCallback
{
    public enum EventCodes
    {
        CheckersCreate = 1,
        CheckersUpdate = 2,
        CheckerStructure = 3,
        OnitamaCreate = 4,
        ChessCreate = 5
    }
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public GameObject piecePrefabLight;
    public GameObject piecePrefabDark;
    public Checkers.GameManager checkersGameManager;
    public ChessScripts.GameManager chessGameManager;
    public GameObject blueStudent;
    public GameObject blueMaster;
    public GameObject redStudent;
    public GameObject redMaster;
    
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    
    
    public GameObject CreateMultiplayerObject(GameObject PlayerPrefab, object[] additionalData = null)
    {
      
        GameObject multiplayerObject = Instantiate(PlayerPrefab);
        PhotonView photonView = multiplayerObject.GetComponent<PhotonView>();
        if (photonView == null)
        {
           photonView = multiplayerObject.AddComponent<PhotonView>();
        }
      
        if (PhotonNetwork.AllocateViewID(photonView))
        {
           
           object[] data = new object[]
           {
               multiplayerObject.transform.position, multiplayerObject.transform.rotation, photonView.ViewID
           };
           if (additionalData != null)
           {
               List<object> dataList = data.ToList();
               // First additional data decides which function will be called
               for (int i = 1; i < additionalData.Length; i++)
               {
                   dataList.Add(additionalData[i]);
               }
              
               data = dataList.ToArray();

           }
           RaiseEventOptions raiseEventOptions = new RaiseEventOptions
           {
               Receivers = ReceiverGroup.Others,
               CachingOption = EventCaching.AddToRoomCache
           };

           SendOptions sendOptions = new SendOptions
           {
               Reliability = true
           };
        
           if (additionalData != null)
           {
          
               PhotonNetwork.RaiseEvent((byte)additionalData[0], data, raiseEventOptions, sendOptions);
           }
           
           photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
           AddTracker(multiplayerObject);
            return multiplayerObject;
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewId.");

            Destroy(multiplayerObject);
            return null;
        }
  
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)EventCodes.CheckersCreate)
        {
            CreateCheckersObject(photonEvent);
        }
        else if (photonEvent.Code == (byte)EventCodes.CheckersUpdate)
        {
            UpdateCheckers(photonEvent);
        }
        else if (photonEvent.Code == (byte)EventCodes.CheckerStructure)
        {
            ReadPieceStructure(photonEvent);
        }
        else if (photonEvent.Code == (byte)EventCodes.OnitamaCreate)
        {
            CreateOnitamaObject(photonEvent);
        }
        else if (photonEvent.Code == (byte)EventCodes.ChessCreate)
        {
            CreateChessObject(photonEvent);
        }
    }
    private void ReadPieceStructure(EventData photonEvent)
    {
        object[] data = (object[]) photonEvent.CustomData;
        if(data != null)
        {
        
            int x = (int) data[0];
            int y = (int) data[1];
            if (data[2] == null)
            {
                Debug.Log( x+ " " +y+" null data received");
                Checkers.GameManager.Instance.pieces[x, y] = null;
                return;
            }
            int viewID = (int) data[3];
            PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
         
            if (photonView == null)
            {
                Checkers.GameManager.Instance.pieces[x, y] = null;
                return;
            }

            bool isPhotonIdExists = false;
            foreach (var piece in checkersGameManager.pieces)
            {
                if (piece!= null && piece.photonID == viewID)
                {
                    isPhotonIdExists = true;
                    Debug.Log( x+ " " +y+"  " + viewID+ "  data received");
                    Checkers.GameManager.Instance.pieces[x, y] = piece;
                    break;
                }
            }

            if (!isPhotonIdExists)
            {
                Debug.Log("New piece received " + viewID);
                Checkers.GameManager.Instance.pieces[x, y] = new Piece(
                    null, 
                    PieceType.Man,
                    Checkers.GameManager.Instance.playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black,
                    false
                    );

                Checkers.GameManager.Instance.pieces[x, y].photonID = viewID;
            }

        }
    }

    private void UpdateCheckers(EventData photonEvent)
    {
        object[] data = (object[]) photonEvent.CustomData;
        if(data != null)
        {
            checkersGameManager.currentPlayer = (PlayerType) data[0];
             checkersGameManager.isAnyPieceTaken = (bool)data[1];
             //checkersGameManager.currentAction =(ActionType) data[2];
             checkersGameManager.currentAction =
                 checkersGameManager.SetAllPieceActions(checkersGameManager.currentPlayer);
             checkersGameManager.isGameFinished = (bool) data[3];
        }
    }

    private void CreateCheckersObject(EventData photonEvent)
    {
        object[] data = (object[]) photonEvent.CustomData;
        GameObject prefab;
        if ((bool) data[3])
        {
            prefab = piecePrefabLight;
        }
        else
        {
            prefab = piecePrefabDark;
        }
        GameObject multiplayerObject = (GameObject) Instantiate(prefab, (Vector3) data[0], (Quaternion) data[1]);
        PhotonView photonView = multiplayerObject.GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = multiplayerObject.AddComponent<PhotonView>();
        }
      //  multiplayerObject.GetComponent<Renderer>().sharedMaterial = (Material)data[3];
        photonView.ViewID = (int) data[2];
        AddTracker(multiplayerObject);
    }
    private void CreateChessObject(EventData photonEvent)
    {
        object[] data = (object[]) photonEvent.CustomData;
        Vector3 position = (Vector3) data[3];
    
        GameObject multiplayerObject =  chessGameManager.GetObjectFromBoard(ChessScripts.GameManager.CalculateBoardPosition(position)).gameObject;
 
        PhotonView photonView = multiplayerObject.GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = multiplayerObject.AddComponent<PhotonView>();
        }
        //  multiplayerObject.GetComponent<Renderer>().sharedMaterial = (Material)data[3];
        photonView.ViewID = (int) data[2];
        AddTracker(multiplayerObject);
    }

    private void CreateOnitamaObject(EventData photonEvent)
    {
        object[] data = (object[]) photonEvent.CustomData;
        GameObject prefab;
        if ((int) data[3] == 1)
        {
            prefab = blueMaster;
        }
        else if ((int) data[3] == 2) 
        {
            prefab = blueStudent;
        }
        else if ((int) data[3] == 3) 
        {
            prefab = redMaster;
        }
        else 
        {
            prefab = redStudent;
        }
        GameObject multiplayerObject = (GameObject) Instantiate(prefab, (Vector3) data[0], (Quaternion) data[1]);
        PhotonView photonView = multiplayerObject.GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = multiplayerObject.AddComponent<PhotonView>();
        }
        //  multiplayerObject.GetComponent<Renderer>().sharedMaterial = (Material)data[3];
        photonView.ViewID = (int) data[2];
        AddTracker(multiplayerObject);
    }

    public void AddTracker(GameObject multiplayerObject)
    {
        if(multiplayerObject == null) return;
        PhotonTransformView photonTransformView =  multiplayerObject.AddComponent<PhotonTransformView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
    
        multiplayerObject.GetComponent<PhotonView>().ObservedComponents = new List<Component>();
        multiplayerObject.GetComponent<PhotonView>().ObservedComponents.Add(photonTransformView);
    }

    public void CreateEvent(byte eventCode,object[]sendData,ReceiverGroup receiverGroup,EventCaching eventCaching = EventCaching.DoNotCache)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup ,  CachingOption = eventCaching }; 
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(eventCode, sendData, raiseEventOptions, sendOptions);
    }
}
