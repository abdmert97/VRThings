using System.Collections.Generic;
using Onitama;
using Photon.Pun;
using UnityEngine;

namespace ChessScripts
{
    public class Controller : MonoBehaviour
    {
        private Camera camera;
        [SerializeField] GreenPointPool greenPointPool;
        private const string chessman = "Chessman";
        private const string move = "Move";
        private Chessman lastSelected;
        private LayerMask Defaultlayer;
        private LayerMask Blacklayer;
        private LayerMask Whitelayer;
        private LayerMask GreenLayer;
        [SerializeField] GameManager gameManager;
        [SerializeField] CommandManager commandManager;
     
        [SerializeField] Material SelectedChessmanColor;
        private List<Chessman> colorChangedChessMans = new List<Chessman>();
        void Start()
        {
            GreenLayer = LayerMask.GetMask("GreenLayer");
            Defaultlayer = LayerMask.GetMask("Default");
            Blacklayer = LayerMask.GetMask("BlackLayer");
            Whitelayer = LayerMask.GetMask("WhiteLayer");
            camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if(!gameManager.isVR)
                if (Input.GetMouseButtonDown(0))
                    GetRaycast();
        }
        void GetRaycast()
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            LayerMask currentLayer = gameManager.TurnForBlack == true ? Blacklayer : Whitelayer;
            if (Physics.Raycast(ray, out hit, 100f, currentLayer))
            {
                SelectChessman(hit);
            }
            else if (Physics.Raycast(ray, out hit, 100f, GreenLayer) && lastSelected)
            {
                MoveChessman(hit);
                gameManager.NextTurn();
                greenPointPool.FreePool();
                ClearColoredChessmans();
            }
            else
            {
                lastSelected = null;
                greenPointPool.FreePool();
                ClearColoredChessmans();
            }
        }

        private void SelectChessman(RaycastHit hit)
        {
            if (hit.collider.CompareTag(chessman))
            {
                Chessman chessman = hit.collider.GetComponent<Chessman>();
                Chessman selectedKing = chessman.isBlack ? gameManager.blackKing : gameManager.whiteKing;
                if (gameManager.isMultiplayer)
                {
                    if (!gameManager.isBlack == chessman.isBlack)
                    {
                        return;
                    }
                }
                lastSelected = chessman;
                List<Position> moves = chessman.AvailableMoves();

                CreateGreenPoints(chessman, moves);

                SFXManager.Instance.Piece_Picked();
            }
        }

        private void MoveChessman(RaycastHit hit)
        {
            SFXManager.Instance.Piece_Dropped();

            Position pos = GameManager.CalculateBoardPosition(hit.collider.transform.position);
            Position lastPos =new Position();
            lastPos.PosX = lastSelected.currentPos.PosX;
            lastPos.PosZ = lastSelected.currentPos.PosZ;
            if (gameManager.Board[pos.PosX, pos.PosZ] != null)
            {
                GameObject eaten = gameManager.Board[pos.PosX, pos.PosZ].gameObject;
                eaten.SetActive(false);
                if(gameManager.isMultiplayer)
                    gameManager.photonView.RPC("EatMulti",RpcTarget.Others,pos.PosX, pos.PosZ);
                commandManager.AddCommand(lastSelected.gameObject, lastSelected.currentPos, pos, eaten);
                gameManager.Board[pos.PosX, pos.PosZ] = null;
                if(gameManager.isMultiplayer)
                    gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,pos.PosX, pos.PosZ,-1);
            }
            else
            {
                commandManager.AddCommand(lastSelected.gameObject, lastSelected.currentPos, pos);
            }
            gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] = null;
            if(gameManager.isMultiplayer)
                gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ,-1);
            lastSelected.Move(lastSelected.currentPos, pos);
            gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] = lastSelected;
            if(gameManager.isMultiplayer)
                gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ,lastSelected.GetComponent<PhotonView>().ViewID);
            lastSelected.firstMove = false;
            if (lastSelected.chessmanType == Chessman.ChessmanType.PAWN)
            {
                //Pawn promote
                if (lastSelected.isBlack == true && lastSelected.currentPos.PosZ == 7)
                {
                    lastSelected.gameObject.SetActive(false);
                    if(gameManager.isMultiplayer)
                        gameManager.photonView.RPC("EatMulti",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ);
                    GameObject chssman = gameManager.spawner.spawnChessman(Chessman.ChessmanType.QUEEN, GameManager.CalculateWorldPosition(pos), true);
                    gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] =
                        chssman.GetComponent<Chessman>();
                }
                else if (lastSelected.isBlack == false && lastSelected.currentPos.PosZ == 0)
                {
                    lastSelected.gameObject.SetActive(false);
                    if(gameManager.isMultiplayer)
                        gameManager.photonView.RPC("EatMulti",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ);
                    GameObject chssman = gameManager.spawner.spawnChessman(Chessman.ChessmanType.QUEEN, GameManager.CalculateWorldPosition(pos), false);
                    gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] =
                        chssman.GetComponent<Chessman>();
                }
            }

            if (lastSelected.chessmanType == Chessman.ChessmanType.KING)
            {
                if (Mathf.Abs(lastSelected.currentPos.PosX-lastPos.PosX)>1)
                {
                    if (lastSelected.currentPos.PosX - lastPos.PosX > 1) // Short rook
                    {
                        Chessman rook = gameManager.Board[7, lastPos.PosZ];
                        rook.firstMove = false;
                        gameManager.Board[7, lastPos.PosZ] = null;
                        gameManager.Board[lastPos.PosX + 1, lastPos.PosZ] = rook;
                        Position targetPos =new Position();
                        targetPos.PosX = lastPos.PosX + 1;
                        targetPos.PosZ =lastPos.PosZ;
                        rook.Move(rook.currentPos,targetPos);

                    }
                    else // Long Rook
                    {
                        Chessman rook = gameManager.Board[0, lastPos.PosZ];
                        rook.firstMove = false;
                        gameManager.Board[0, lastPos.PosZ] = null;
                        gameManager.Board[lastPos.PosX - 1, lastPos.PosZ] = rook;    
                        Position targetPos =new Position();
                        targetPos.PosX = lastPos.PosX - 1;
                        targetPos.PosZ =lastPos.PosZ;
                        rook.Move(rook.currentPos,targetPos);
                    }
                }
            }
        }

        private void CreateGreenPoints(Chessman chessman, List<Position> moves)
        {
            ClearColoredChessmans();
            greenPointPool.FreePool();
            bool eated = false;
            foreach (Position move in moves)
            {
                if (gameManager.isValidMove(move, lastSelected.isBlack, ref eated))
                {
                    if (!gameManager.isValidForKing(move, chessman))
                    {
                        continue;
                    }
                    if (eated)
                    {
                        Chessman coloredChessman = gameManager.GetObjectFromBoard(move).GetComponent<Chessman>();
                        coloredChessman.renderer.sharedMaterial = SelectedChessmanColor;
                        colorChangedChessMans.Add(coloredChessman);
                    }
                    greenPointPool.GetGreenPoint(GameManager.CalculateWorldPosition(move) + Vector3.up * 0.01f);
                }
            }
        }
        private void ClearColoredChessmans()
        {

            foreach (Chessman chessman in colorChangedChessMans)
            {
                chessman.renderer.sharedMaterial = chessman.defaultMaterial;
            }
            colorChangedChessMans.Clear();
        }

        public void PickPieceVR(GameObject piece)
        {
            Chessman chessman = piece.GetComponent<Chessman>();
            Chessman selectedKing = chessman.isBlack ? gameManager.blackKing : gameManager.whiteKing;

            gameManager.vrStates.PickPiece(chessman.gameObject.GetComponent<BoxCollider>());
            lastSelected = chessman;
            List<Position> moves = chessman.AvailableMoves();

            CreateGreenPoints(chessman, moves);
        }

        public void DropPieceVR(Transform moveCell)
        {
            gameManager.vrStates.DropPiece();
            
            if(moveCell == null)
            {
                lastSelected.Move(lastSelected.currentPos, lastSelected.currentPos);
                lastSelected = null;
                greenPointPool.FreePool();
                ClearColoredChessmans();
                return;
            }

            MoveChessmanVR(moveCell);
            gameManager.NextTurn();
            greenPointPool.FreePool();
            ClearColoredChessmans();
        }

        private void MoveChessmanVR(Transform hit)
        {
            Position pos = GameManager.CalculateBoardPosition(hit.position);

            if (gameManager.Board[pos.PosX, pos.PosZ] != null)
            {
                GameObject eaten = gameManager.Board[pos.PosX, pos.PosZ].gameObject;
                eaten.SetActive(false);
                if(gameManager.isMultiplayer)
                    gameManager.photonView.RPC("EatMulti",RpcTarget.Others,pos.PosX, pos.PosZ);
                commandManager.AddCommand(lastSelected.gameObject, lastSelected.currentPos, pos, eaten);
                gameManager.Board[pos.PosX, pos.PosZ] = null;
                if(gameManager.isMultiplayer)
                    gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,pos.PosX, pos.PosZ,-1);
            }
            else
            {
                commandManager.AddCommand(lastSelected.gameObject, lastSelected.currentPos, pos);
            }
            gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] = null;
            if(gameManager.isMultiplayer)
                gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ,-1);
            lastSelected.Move(lastSelected.currentPos, pos);
            gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] = lastSelected;
            if(gameManager.isMultiplayer)
                gameManager.photonView.RPC("UpdateMultiBoard",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ,lastSelected.GetComponent<PhotonView>().ViewID);
            lastSelected.firstMove = false;
            if (lastSelected.chessmanType == Chessman.ChessmanType.PAWN)
            {
                //Pawn promote
                if (lastSelected.isBlack == true && lastSelected.currentPos.PosZ == 7)
                {
                    lastSelected.gameObject.SetActive(false);
                    if(gameManager.isMultiplayer)
                        gameManager.photonView.RPC("EatMulti",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ);
                    GameObject chssman = gameManager.spawner.spawnChessman(Chessman.ChessmanType.QUEEN, GameManager.CalculateWorldPosition(pos), true);
                    gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] =
                        chssman.GetComponent<Chessman>();
                }
                else if (lastSelected.isBlack == false && lastSelected.currentPos.PosZ == 0)
                {
                    lastSelected.gameObject.SetActive(false);
                    if(gameManager.isMultiplayer)
                        gameManager.photonView.RPC("EatMulti",RpcTarget.Others,lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ);
                    GameObject chssman = gameManager.spawner.spawnChessman(Chessman.ChessmanType.QUEEN, GameManager.CalculateWorldPosition(pos), false);
                    gameManager.Board[lastSelected.currentPos.PosX, lastSelected.currentPos.PosZ] =
                        chssman.GetComponent<Chessman>();
                }
            }
        }

    }
}