using ComplexGame.Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComplexGame
{
    public class CardOption : MonoBehaviour
    {
        public void FirstOption()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                InGameUIManager.Instance.selectedOption = 1;
                var player = GameManager.Instance.GetCurrentPlayer().GetComponent<Player>();
                var discardPileCards = player.discardPileClass.cards;
                var lastPlayedCard = discardPileCards[discardPileCards.Count - 1];
                if (lastPlayedCard.cardType == CardType.Reproduction)
                {
                    if (((Reproduction) lastPlayedCard).PlayOption1(player, out var result))
                    {
                        AnimatedCanvas.Instance.ShowMessage(result);
                    }
                    else
                    {
                        AnimatedCanvas.Instance.ShowError(result);
                    }
                }
                else if (lastPlayedCard.cardType == CardType.Gathering)
                {
                    if (((Gathering) lastPlayedCard).PlayOption1(player, out var result))
                    {
                        AnimatedCanvas.Instance.ShowMessage(result);
                    }
                    else
                    {
                        AnimatedCanvas.Instance.ShowError(result);
                    }
                }
            }
        }

        public void SecondOption()
        {
            InGameUIManager.Instance.selectedOption = 2;
            var player = GameManager.Instance.GetCurrentPlayer().GetComponent<Player>();
            var discardPileCards = player.discardPileClass.cards;
            var lastPlayedCard = discardPileCards[discardPileCards.Count - 1];
            if (lastPlayedCard.cardType == CardType.Reproduction)
            {
                if (((Reproduction) lastPlayedCard).PlayOption2(player, out var result))
                {
                    AnimatedCanvas.Instance.ShowMessage(result);
                }
                else
                {
                    AnimatedCanvas.Instance.ShowError(result);
                }
            }
            else if (lastPlayedCard.cardType == CardType.Gathering)
            {
                if (((Gathering) lastPlayedCard).PlayOption2(player, out var result))
                {
                    AnimatedCanvas.Instance.ShowMessage(result);
                }
                else
                {
                    AnimatedCanvas.Instance.ShowError(result);
                }
            }
        }
    }
}
