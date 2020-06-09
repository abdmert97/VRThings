using System.Collections.Generic;
using CommonScripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ChessScripts
{
    public class GameManager : MonoBehaviour
    {
        
        public const float TILE_SIZE = 6;
        public static Vector3 startPoint = new Vector3(-21, 0, -21);
        public Chessman[,] Board = new Chessman[8, 8];
        public Chessman blackKing;
        public Chessman whiteKing;
        public CommandManager commandManager;
        public Spawner spawner;
        public bool TurnForBlack = false;
        public bool isMultiplayer ;
        [SerializeField] Transform blackCameraTransform;
        [SerializeField] Transform whiteCameraTransform;
        [SerializeField] GameObject endGamePanel;
      
        public Material SelectedChessmanMaterial;
        private Camera cam;
        public Timer timer;
        public PhotonView photonView;
        public bool isVR;
        public bool isBlack;
        public VRStates vrStates = new VRStates();
        void Init()
        {
            isMultiplayer = MultiplayerSetup.Instance.IsMultiplayerGame;
            TurnForBlack = false;
            cam = Camera.main;
            spawner.SpawnAll(startPoint, TILE_SIZE);
            if (isMultiplayer)
            {
                photonView = gameObject.GetComponent<PhotonView>();
                
            }
            if(isVR)
            {
                List<BoxCollider> colliders = new List<BoxCollider>();

                for(int i=0; i<8; i++)
                {
                    colliders.Add(Board[i,0].gameObject.GetComponent<BoxCollider>());
                    colliders.Add(Board[i,1].gameObject.GetComponent<BoxCollider>());
                    colliders.Add(Board[i,6].gameObject.GetComponent<BoxCollider>());
                    colliders.Add(Board[i,7].gameObject.GetComponent<BoxCollider>());
                }
                
                int colliderCount = colliders.Count;

                for(int i=0; i<colliderCount; i++)
                {
                    for(int j=i+1; j<colliderCount; j++)
                    {
                        Physics.IgnoreCollision(colliders[i],colliders[j]);
                    }
                }

                AdjustGrabbableVR(TurnForBlack);
            }
        }
     
        private void OnEnable()
        {
            CommandManager.UpdateBoard += UpdateBoard;
            CommandManager.TurnChanged += NextTurn;
            Timer.EndTimer += WinGameFor;
            if (isMultiplayer)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    isBlack = false;
                }
                else
                {
                    cam.transform.position = blackCameraTransform.position;
                    cam.transform.rotation = blackCameraTransform.rotation;
                    isBlack = true;
                }
            }
        }
        private void OnDisable()
        {
            CommandManager.UpdateBoard -= UpdateBoard;
            CommandManager.TurnChanged -= NextTurn;
            Timer.EndTimer -= WinGameFor;

        }
        private void Awake()
        {
            Init();
        }
        public static Vector3 CalculateWorldPosition(Position pos)
        {
            return pos.PosX * Vector3.right * TILE_SIZE + Vector3.forward * pos.PosZ * TILE_SIZE + startPoint;
        }
        public static Position CalculateBoardPosition(Vector3 pos)
        {
            Position boardPos = new Position();
            boardPos.PosX = (int)((pos.x - startPoint.x) / TILE_SIZE);
            boardPos.PosZ = (int)((pos.z - startPoint.z) / TILE_SIZE);

            return boardPos;
        }
        public static bool IsValidPosition(Position pos)
        {
            if (pos.PosX >= 8)
                return false;
            if (pos.PosZ >= 8)
                return false;
            if (pos.PosX < 0)
                return false;
            if (pos.PosZ < 0)
                return false;
            return true;
        }
        public bool isValidMove(Position pos, bool isBlack, ref bool canBeEaten)
        {
            Chessman chessman = Board[pos.PosX, pos.PosZ]?.GetComponent<Chessman>();
            canBeEaten = false;
            if (chessman == null) return true;
            if (chessman.isBlack == isBlack)
            {
                return false;
            }
            else
            {
                canBeEaten = true;
                return true;
            }
        }
        public bool CheckBoard(Position pos)
        {
            return Board[pos.PosX, pos.PosZ] == null ? false : true;
        }
        
        public void NextTurn()
        {
            if(isMultiplayer)
                photonView.RPC("NextTurnOthers", RpcTarget.Others);
            TurnForBlack = !TurnForBlack;
            if (IsGameEnded(TurnForBlack))
            {
                if (TurnForBlack)
                {
                    WinGameFor(1);
                }
                else
                {
                    WinGameFor(0);
                }
            }
            TimerChange();

            if(!isVR)
            {
                Invoke("ChangeCamera", 1f);
            }

            if(isVR)
            {
                AdjustGrabbableVR(TurnForBlack);
            }
        }
        [PunRPC]
        public void NextTurnOthers()
        {
            TurnForBlack = !TurnForBlack;
            if (IsGameEnded(TurnForBlack))
            {
          
                if (TurnForBlack)
                    WinGameFor(1);
                else
                {
                    WinGameFor(0);
                }
            }
            TimerChange();

            if(!isVR)
            {
                Invoke("ChangeCamera", 1f);
            }

            if(isVR)
            {
                AdjustGrabbableVR(TurnForBlack);
            }
        }

        [PunRPC]
        public void UpdateMultiBoard(int x, int y, int pID)
        {
            if (pID == -1)
            {
                Board[x, y] = null;
            }
            else
            {
                Board[x, y] = PhotonView.Find(pID).GetComponent<Chessman>();
            }
        }
        [PunRPC]
        public void EatMulti(int x, int y)
        {
            Board[x,y].gameObject.SetActive(false);
            Board[x, y] = null;
        }
        private void TimerChange()
        {
            timer.StartCondition = true;

            timer.timers[1].isTimerEnabled = TurnForBlack ? true : false;
            timer.timers[0].isTimerEnabled = TurnForBlack ? false : true;
            if(TurnForBlack)
            {
                timer.TurnOffDisplay(0);
                timer.TurnOnDisplay(1);
            }
            else
            {
                timer.TurnOnDisplay(0);
                timer.TurnOffDisplay(1);
            }
        }

        public void ChangeCamera()
        {
            if(isMultiplayer)
                return;
            if (TurnForBlack)
            {
                cam.transform.position = blackCameraTransform.position;
                cam.transform.rotation = blackCameraTransform.rotation;
            }
            else
            {
                cam.transform.position = whiteCameraTransform.position;
                cam.transform.rotation = whiteCameraTransform.rotation;
            }
        }
        public bool CheckMate()
        {
            return false;
        }
        public void UpdateBoard(Position pos, Chessman chessman)
        {
            Board[pos.PosX, pos.PosZ] = chessman;
        }

        public void LoadMainScene()
        {
            UIManager.UIManager.Instance.LoadScene(0);
        }
        //Debug
        public void PrintBoard()
        {
            string message = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    message += (i.ToString() + " " + j.ToString() + " ");
                    message += Board[j, i] ? Board[j, i].name : " empty ";
                }
                Debug.Log(message);
                message = "";
            }
        }
        public Chessman GetObjectFromBoard(Position pos)
        {
            return Board[pos.PosX, pos.PosZ];
        }
        public void RestartGame()
        {
            if (TurnForBlack)
            {
                TurnForBlack = false;
                ChangeCamera();
            }
            timer.RestarTimerIndex(0);
            timer.RestarTimerIndex(1);
            timer.TurnOnDisplay(0);
            Board = new Chessman[8, 8];
            commandManager.ClearCommands();
            spawner.ClearChessmans();
            spawner.SpawnAll(startPoint, TILE_SIZE);

        }
        public bool IsGameEnded(bool isBlack)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int z = 0; z < 8; z++)
                {
                    Chessman chessman = Board[x, z];
                    if (chessman == null) continue;
                    if (chessman.isBlack != isBlack) continue;
                    List<Position> availableMoves = chessman.AvailableMoves();
                    foreach (Position availableMove in availableMoves)
                    {
                        if (isValidForKing(availableMove, chessman))
                            return false;
                    }
                }
            }

            
            SFXManager.Instance.Game_Finished();

            return true;
        }
        public bool isValidForKing(Position move, Chessman chessman)
        {
            // Simulate move 
            bool valid = true;
            UpdateBoard(chessman.currentPos, null);
            Chessman temp = GetObjectFromBoard(move);
            UpdateBoard(move, chessman);
            for (int x = 0; x < 8 && valid; x++)
            {
                for (int z = 0; z < 8 && valid; z++)
                {
                    Chessman controlCheckMate = Board[x, z];
                    if (controlCheckMate == null) continue;
                    if (controlCheckMate.isBlack == chessman.isBlack) continue;

                    List<Position> availableMoves = controlCheckMate.AvailableMoves();

                    foreach (Position availableMove in availableMoves)
                    {
                        Chessman eat = GetObjectFromBoard(availableMove);

                        if (eat == null) continue;

                        if (eat.isBlack != chessman.isBlack) continue;

                        if (eat.chessmanType == Chessman.ChessmanType.KING)
                        {
                            valid = false;
                        }
                    }
                }
            }
            UpdateBoard(chessman.currentPos, chessman);
            UpdateBoard(move, temp);
            return valid;
        }   
        [PunRPC]
        public void WinGameForMulti(int index)
        {
            timer.PauseTimer(0);
            timer.PauseTimer(1);
            timer.TurnOffDisplay(0);
            timer.TurnOffDisplay(1);
       
            UIManager.UIManager.Instance.OpenPanel(endGamePanel);
            endGamePanel.GetComponentInChildren<TextMeshProUGUI>().text += index == 0 ? "\n Black Wins" : "\n White Wins";
            if (index == 0)
                Debug.Log("Black Wins");
            else
            {
                Debug.Log("White Wins");
            }
            //Time.timeScale = 0;
        }
        public void WinGameFor(int index)
        {
            if (isMultiplayer)
            {
                photonView.RPC("WinGameForMulti", RpcTarget.Others,index);
            }
            timer.PauseTimer(0);
            timer.PauseTimer(1);
            timer.TurnOffDisplay(0);
            timer.TurnOffDisplay(1);
       
            UIManager.UIManager.Instance.OpenPanel(endGamePanel);
            endGamePanel.GetComponentInChildren<TextMeshProUGUI>().text += index == 0 ? "\n Black Wins" : "\n White Wins";
            if (index == 0)
                Debug.Log("Black Wins");
            else
            {
                Debug.Log("White Wins");
            }
            //Time.timeScale = 0;
        }

        private void AdjustGrabbableVR(bool isBlack)
        {
            vrStates.Clear();
            
            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    Chessman piece = Board[i,j];
                    if(piece != null)
                    {
                        //piece.gameObject.GetComponent<BoxCollider>().enabled = piece.isBlack == isBlack ? true : false;
                        
                        Collider collider = piece.gameObject.GetComponent<BoxCollider>();

                        if(piece.isBlack == isBlack)
                        {
                            collider.enabled = true;
                            vrStates.Add(collider);
                        }
                        else
                        {
                            collider.enabled = false;
                        }
                    }
                }
            }
        }
    }
}