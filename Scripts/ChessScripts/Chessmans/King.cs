using System.Collections.Generic;

namespace ChessScripts.Chessmans
{
    public class King : Chessman
    {

        public override List<Position> AvailableMoves()
        {
            int forward = 1;
            availableMoves.Clear();
            List<Position> moves = availableMoves;
            bool canBeEaten = false;
            Position move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);

            }

            move = new Position();
            move.PosX = currentPos.PosX;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ - forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            move = new Position();
            move.PosX = currentPos.PosX;
            move.PosZ = currentPos.PosZ - forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }
            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ - forward;
            if (GameManager.IsValidPosition(move))
            {
                if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }
            
            // Try Rook
            if(firstMove)
            {
                
                move = new Position();
                move.PosX = currentPos.PosX - 2 * forward;
                move.PosZ = currentPos.PosZ;
                if (gameManager.Board[move.PosX - 1*forward, move.PosZ]&&gameManager.Board[move.PosX - 1*forward, move.PosZ].firstMove) // Rook first move
                {
                    if (gameManager.Board[move.PosX + 1*forward, move.PosZ] == null)
                    {
                        if (GameManager.IsValidPosition(move))
                        {
                            if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                                moves.Add(move);
                        }
                    }
                }
                
                move = new Position();
                move.PosX = currentPos.PosX + 2 * forward;
                move.PosZ = currentPos.PosZ;
                if (gameManager.Board[move.PosX + 2*forward, move.PosZ]&&gameManager.Board[move.PosX +2*forward, move.PosZ].firstMove) // Rook first move
                {
                    if (gameManager.Board[move.PosX + 1*forward, move.PosZ] == null&&gameManager.Board[move.PosX - 1*forward, move.PosZ] == null)
                    {
                        if (GameManager.IsValidPosition(move))
                        {
                            if (gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                                moves.Add(move);
                        }
                    }
                }
              

               
            }
            return moves;
        }


    }
}