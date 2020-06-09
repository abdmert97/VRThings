using System.Collections.Generic;
using UnityEngine;

namespace ChessScripts
{
    public class CommandManager : MonoBehaviour
    {


        public List<Command> commandList = new List<Command>();
        public List<Command> redoCommandList = new List<Command>();

        public static event System.Action<Position, Chessman> UpdateBoard;
        public static event System.Action TurnChanged;

        public void UndoCommand()
        {
            if (commandList.Count == 0) return;
            Command command = commandList[commandList.Count - 1];
            command.UndoCommand();
            redoCommandList.Add(command);
            commandList.RemoveAt(commandList.Count - 1);
            UpdateBoard.Invoke(command.currPosition, null);
            Chessman chessman = command.chessman.GetComponent<Chessman>();
            chessman.currentPos = command.prevPosition;
            if (commandList.Count < 2) chessman.firstMove = true;
            UpdateBoard.Invoke(command.prevPosition, chessman);
            if (command.eaten)
            {
                Chessman eaten = command.eaten.GetComponent<Chessman>();
                UpdateBoard.Invoke(command.currPosition, eaten);
                eaten.currentPos = command.currPosition;
            }
            TurnChanged.Invoke();
        }
        public void RedoCommand()
        {
            if (redoCommandList.Count == 0) return;
            Command command = redoCommandList[redoCommandList.Count - 1];
            command.RedoCommand();
            commandList.Add(command);
            redoCommandList.RemoveAt(redoCommandList.Count - 1);
            UpdateBoard.Invoke(command.currPosition, command.chessman.GetComponent<Chessman>());
            UpdateBoard.Invoke(command.prevPosition, null);
            TurnChanged.Invoke();
        }
        public void AddCommand(GameObject chessman, Position prevPosition, Position nextPositon, GameObject eaten = null)
        {
            Command command = new Command(chessman, prevPosition, nextPositon, eaten);

            commandList.Add(command);
        }
        public void ClearCommands()
        {
            commandList.Clear();
            redoCommandList.Clear();

        }
    }
    public struct Command
    {
        public GameObject chessman;
        public Position prevPosition;
        public Position currPosition;
        public GameObject eaten;


        public override string ToString()
        {
            return chessman.name + " from " + prevPosition + " to " + currPosition;
        }
        public Command(GameObject _chessman, Position _prevPosition, Position _currPosition,
            GameObject _eaten)
        {
            chessman = _chessman;
            prevPosition = _prevPosition;
            currPosition = _currPosition;
            eaten = _eaten;

        }
        public void RedoCommand()
        {
            if (eaten)
            {
                eaten.SetActive(false);
            }
            chessman.transform.position = GameManager.CalculateWorldPosition(currPosition);

        }
        public void UndoCommand()
        {
            if (eaten)
            {
                eaten.SetActive(true);
            }
            chessman.transform.position = GameManager.CalculateWorldPosition(prevPosition);
        }

    }
}