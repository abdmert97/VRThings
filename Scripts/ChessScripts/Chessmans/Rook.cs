using System.Collections.Generic;

namespace ChessScripts.Chessmans
{
    public class Rook : Chessman
    {
        public override List<Position> AvailableMoves()
        {
            int forward = 1;
            bool canBeEaten = false;
            availableMoves.Clear();
            List<Position> moves = availableMoves;
            for (int i = 1; i < 8; i++)
            {

                Position move = new Position();
                move.PosX = currentPos.PosX;
                move.PosZ = currentPos.PosZ + forward * i;
                if (GameManager.IsValidPosition(move))
                {
                    if (!gameManager.isValidMove(move, isBlack, ref canBeEaten))
                    {
                        if (canBeEaten)
                            moves.Add(move);
                        break;
                    }
                    if (canBeEaten)
                    {
                        moves.Add(move);
                        break;
                    }
                    moves.Add(move);
                }
            }
            for (int i = -1; i > -8; i--)
            {

                Position move = new Position();
                move.PosX = currentPos.PosX;
                move.PosZ = currentPos.PosZ + forward * i;
                if (GameManager.IsValidPosition(move))
                {
                    if (!gameManager.isValidMove(move, isBlack, ref canBeEaten))
                    {
                        if (canBeEaten)
                            moves.Add(move);
                        break;
                    }
                    if (canBeEaten)
                    {
                        moves.Add(move);
                        break;
                    }
                    moves.Add(move);
                }
            }
            for (int i = 1; i < 8; i++)
            {

                Position move = new Position();
                move.PosX = currentPos.PosX + forward * i;
                move.PosZ = currentPos.PosZ;
                if (GameManager.IsValidPosition(move))
                {
                    if (!gameManager.isValidMove(move, isBlack, ref canBeEaten))
                    {
                        if (canBeEaten)
                            moves.Add(move);
                        break;
                    }
                    if (canBeEaten)
                    {
                        moves.Add(move);
                        break;
                    }
                    moves.Add(move);
                }
            }
            for (int i = -1; i > -8; i--)
            {

                Position move = new Position();
                move.PosX = currentPos.PosX + forward * i;
                move.PosZ = currentPos.PosZ;
                if (GameManager.IsValidPosition(move))
                {
                    if (!gameManager.isValidMove(move, isBlack, ref canBeEaten))
                    {
                        if (canBeEaten)
                            moves.Add(move);
                        break;
                    }
                    if (canBeEaten)
                    {
                        moves.Add(move);
                        break;
                    }
                    moves.Add(move);
                }
            }

            return moves;
        }


    }
}