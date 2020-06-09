using System.Collections.Generic;

namespace ChessScripts.Chessmans
{
    public class Pawn : Chessman
    {

        public override List<Position> AvailableMoves()
        {

            int forward = isBlack == true ? 1 : -1;
            availableMoves.Clear();
            List<Position> moves = availableMoves;
            Position move = new Position();
            bool canBeEaten = false;
            move.PosX = currentPos.PosX;
            move.PosZ = currentPos.PosZ + forward;
            gameManager.isValidMove(move, isBlack, ref canBeEaten);
            if (GameManager.IsValidPosition(move) && !gameManager.CheckBoard(move))
                moves.Add(move);
            if (firstMove && gameManager.Board[move.PosX, move.PosZ] == null)
            {
                move = new Position();
                move.PosX = currentPos.PosX;
                move.PosZ = currentPos.PosZ + forward * 2;
                gameManager.isValidMove(move, isBlack, ref canBeEaten);
                if (GameManager.IsValidPosition(move) && !gameManager.CheckBoard(move))
                    moves.Add(move);

            }
            move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
            {
                if (!gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }
            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
            {

                if (!gameManager.isValidMove(move, isBlack, ref canBeEaten) || canBeEaten)
                    moves.Add(move);
            }

            return moves;
        }




    }
}