using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessScripts
{
    public abstract class Chessman : MonoBehaviour
    {

        public GameManager gameManager;
        public Renderer renderer;
        public Material defaultMaterial;
        public List<Position> availableMoves;
        private void Awake()
        {
            availableMoves = new List<Position>();
            renderer = GetComponent<Renderer>();
            if (!renderer) renderer = GetComponentInChildren<Renderer>();
            gameManager = FindObjectOfType<GameManager>();
            defaultMaterial = renderer.sharedMaterial;
        }
        public enum ChessmanType { PAWN, BISHOP, KING, KNIGHT, QUEEN, ROOK };
        public bool firstMove;
        public ChessmanType chessmanType;
        public Position currentPos;
        public bool isBlack;
        private WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();
        
        private void OnMouseEnter()
        {
            if (gameManager.isMultiplayer)
            {
                if (gameManager.TurnForBlack == isBlack && gameManager.isBlack == isBlack)
                {
                    renderer.sharedMaterial = gameManager.SelectedChessmanMaterial;
                }
            }
            else
            {
                if (gameManager.TurnForBlack == isBlack)
                {
                    renderer.sharedMaterial = gameManager.SelectedChessmanMaterial;
                }
            }
           
        }

        private void OnMouseExit()
        {
            if (gameManager.TurnForBlack == isBlack)
            {
                renderer.sharedMaterial = defaultMaterial;
            }
        }
        public virtual void Move(Position from, Position to)
        {
            if(gameManager.isVR)
            {
                currentPos = to;
                Vector3 target = GameManager.CalculateWorldPosition(to);
                transform.localPosition = target;
                transform.eulerAngles = chessmanType != ChessmanType.KNIGHT ? new Vector3(-90,0,0) : isBlack ? new Vector3(-90,0,-90) : new Vector3(-90,0,90);
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                rigidbody.velocity = new Vector3(0,0,0);
                rigidbody.angularVelocity = new Vector3(0,0,0);
                return;
            }
            
            currentPos = to;
            StartCoroutine(MoveRoutine(from, to));
        }
        IEnumerator MoveRoutine(Position from, Position to)
        {
            int iteration = 40;
            Vector3 current = GameManager.CalculateWorldPosition(from);
            Vector3 target = GameManager.CalculateWorldPosition(to);
            Vector3 distance = target - current;
            distance /= iteration;
            for (int i = 0; i < iteration; i++)
            {
                transform.localPosition += distance;
                yield return waitFixed;
            }
        }
        public abstract List<Position> AvailableMoves();

        private void Start()
        {
            tag = "Chessman";
            firstMove = true;
        }
    }
    public struct Position
    {
        public int PosX;
        public int PosZ;
        public override string ToString()
        {
            return "Position X: " + PosX.ToString() + " Position Z: " + PosZ.ToString();
        }
        Position(int X,int Z)
        {
            PosX = X;
            PosZ = Z;
        }
        public bool isEqual(Position pos)
        {
            if (pos.PosX == PosX && pos.PosZ == PosZ)
                return true;
            return false;
        }
        public Position Minus(Position pos)
        {
            return new Position(PosX - pos.PosX, PosZ - pos.PosZ);
        }
    }
}