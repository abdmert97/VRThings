using Photon.Pun;
using UnityEngine;

namespace ChessScripts
{
    public class Spawner : MonoBehaviour
    {

        Chessman.ChessmanType chessmanType;
        [SerializeField] GameObject[] chessmanPrefabsBlack;
        [SerializeField] GameObject[] chessmanPrefabsWhite;
        [SerializeField] GameManager gameManager;
        GameObject chessmans;
        public void SpawnAll(Vector3 startPoint, float tile_size)
        {
            //Spawn Pawns
            GameObject board = GameObject.FindGameObjectWithTag("Board");
            chessmans = new GameObject("Chessmans");
            chessmans.transform.SetParent(board.transform);
            GameObject pawns = new GameObject("Pawns");
            GameObject bisops = new GameObject("BiShops");
            GameObject kinghts = new GameObject("Knights");
            GameObject rooks = new GameObject("Rooks");
            pawns.transform.SetParent(chessmans.transform);
            bisops.transform.SetParent(chessmans.transform);
            kinghts.transform.SetParent(chessmans.transform);
            rooks.transform.SetParent(chessmans.transform);

            GameObject temp;

            for (int i = 0; i < 8; i++)
            {
                GameObject pawn;
                pawn = spawnChessman(Chessman.ChessmanType.PAWN, startPoint + Vector3.right * tile_size * i + Vector3.forward * tile_size, true);
                pawn.transform.SetParent(pawns.transform);
                pawn = spawnChessman(Chessman.ChessmanType.PAWN, startPoint + Vector3.right * tile_size * i + Vector3.forward * 6 * tile_size, false);
                pawn.transform.SetParent(pawns.transform);
            }
            //Spawn Bishops
            for (int i = 1; i <= 2; i++)
            {
                int spawnPoint = i == 1 ? 2 : 5;
                GameObject bishop;
                bishop = spawnChessman(Chessman.ChessmanType.BISHOP, startPoint + Vector3.right * tile_size * spawnPoint, true);
                bishop.transform.SetParent(bisops.transform);
                bishop = spawnChessman(Chessman.ChessmanType.BISHOP, startPoint + Vector3.right * tile_size * spawnPoint + Vector3.forward * 7 * tile_size, false);
                bishop.transform.SetParent(bisops.transform);
            }
            // Spawn Kings
            temp = spawnChessman(Chessman.ChessmanType.KING, startPoint + Vector3.right * tile_size * 3, true);
            temp.transform.SetParent(chessmans.transform);
            gameManager.blackKing = temp.GetComponent<Chessman>();
            temp = spawnChessman(Chessman.ChessmanType.KING, startPoint + Vector3.right * tile_size * 3 + Vector3.forward * 7 * tile_size, false);
            temp.transform.SetParent(chessmans.transform);
            gameManager.whiteKing = temp.GetComponent<Chessman>();
            //Spawn Knights
            for (int i = 1; i <= 2; i++)
            {
                int spawnPoint = i == 1 ? 1 : 6;
                GameObject kinght;
                kinght = spawnChessman(Chessman.ChessmanType.KNIGHT, startPoint + Vector3.right * tile_size * spawnPoint, true);
                kinght.transform.SetParent(kinghts.transform);
                kinght.transform.Rotate(Vector3.forward * -90);
                kinght = spawnChessman(Chessman.ChessmanType.KNIGHT, startPoint + Vector3.right * tile_size * spawnPoint + Vector3.forward * 7 * tile_size, false);
                kinght.transform.SetParent(kinghts.transform);
                kinght.transform.Rotate(Vector3.forward * 90);
            }
            //Spawn Queens
            temp = spawnChessman(Chessman.ChessmanType.QUEEN, startPoint + Vector3.right * tile_size * 4, true);
            temp.transform.SetParent(chessmans.transform);
            temp = spawnChessman(Chessman.ChessmanType.QUEEN, startPoint + Vector3.right * tile_size * 4 + Vector3.forward * 7 * tile_size, false);
            temp.transform.SetParent(chessmans.transform);
            for (int i = 1; i <= 2; i++)
            {
                int spawnPoint = i == 1 ? 0 : 7;
                GameObject rook;
                rook = spawnChessman(Chessman.ChessmanType.ROOK, startPoint + Vector3.right * tile_size * spawnPoint, true);
                rook.transform.SetParent(rooks.transform);

                rook = spawnChessman(Chessman.ChessmanType.ROOK, startPoint + Vector3.right * tile_size * spawnPoint + Vector3.forward * 7 * tile_size, false);
                rook.transform.SetParent(rooks.transform);

            }
        }

        public GameObject spawnChessman(Chessman.ChessmanType type, Vector3 position, bool isBlack)
        {
            GameObject chessmanObj;
            if (isBlack)
            {
                if (gameManager.isMultiplayer&&!PhotonNetwork.IsMasterClient)
                {
                    chessmanObj =
                        MultiplayerObjectCreator.Instance.CreateMultiplayerObject(chessmanPrefabsBlack[(int) type],new object[]{(byte)5,position});
                    chessmanObj.transform.position = position;
                  
                }
                else
                {
                    chessmanObj = Instantiate(chessmanPrefabsBlack[(int)type], position, Quaternion.identity);
                    chessmanObj.transform.Rotate(Vector3.right * -90);
                }
                
            }
            else
            {
                if (gameManager.isMultiplayer&&PhotonNetwork.IsMasterClient)
                {
                    chessmanObj =
                        MultiplayerObjectCreator.Instance.CreateMultiplayerObject(chessmanPrefabsWhite[(int) type],new object[]{(byte)5,position});
                    chessmanObj.transform.position = position;
                }
                else
                {
                    chessmanObj = Instantiate(chessmanPrefabsWhite[(int)type], position, Quaternion.identity);
                    chessmanObj.transform.Rotate(Vector3.right * -90);
                }
            }


            Chessman chessman = chessmanObj.GetComponent<Chessman>();
            Position pos = GameManager.CalculateBoardPosition(position);

            gameManager.Board[pos.PosX, pos.PosZ] = chessman;
            chessmanObj.GetComponent<Chessman>().currentPos = GameManager.CalculateBoardPosition(position);
            chessman.isBlack = isBlack;

            return chessmanObj;
        }

        public void ClearChessmans()
        {
            Destroy(chessmans);
        }
        public void RespawnChessmans()
        {
            SpawnAll(GameManager.startPoint, GameManager.TILE_SIZE);
        }

    }
}