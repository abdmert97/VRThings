using System.Collections.Generic;

namespace ChessScripts.Chessmans
{
    public class Knight : Chessman
    {
        public override List<Position> AvailableMoves()
        {
            int forward = 1;
            availableMoves.Clear();
            List<Position> moves = availableMoves;

            Position move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ + 2 * forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ + 2 * forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX - forward;
            move.PosZ = currentPos.PosZ - 2 * forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX + forward;
            move.PosZ = currentPos.PosZ - 2 * forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);


            move = new Position();
            move.PosX = currentPos.PosX + 2 * forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX + 2 * forward;
            move.PosZ = currentPos.PosZ - forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX - 2 * forward;
            move.PosZ = currentPos.PosZ - forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);
            move = new Position();
            move.PosX = currentPos.PosX - 2 * forward;
            move.PosZ = currentPos.PosZ + forward;
            if (GameManager.IsValidPosition(move))
                moves.Add(move);





            return moves;
        }
    }
}