using System;
using UnityEngine;

namespace ComplexGame
{
    public class TurnManager : MonoBehaviour
    {

        public static Action EndTurn;
        public int TurnCount{ get;private set; }
        public int TurnFor { get; private set; }
        private void Start()
        {
            TurnFor = -1;
            NextTurn();
        }
  
        public void NextTurn()
        {
            if(TurnFor != -1)
                GameManager.Instance.playerList[TurnFor].InvokeEndTurn();
            TurnFor++;
            TurnFor = TurnFor % GameManager.Instance.gameData.PlayerCount;
            GameManager.Instance.playerList[TurnFor].InvokePlay();
            InGameUIManager.Instance.CloseInfoPanel();
            
            GameManager.Instance.CloseTileEffect();
            GameManager.Instance.CloseVertexEffect();
            EndTurn?.Invoke();
            
            if(VRManager.instance)
            {
                VRManager.instance.EndTurn();
            }
        }
    }
}
