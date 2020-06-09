    using System;
    using System.Collections.Generic;
    using UnityEngine;

    namespace ComplexGame.Cards
{
    public class Diplomacy : Card
    {
        // TODO: player should choose card actions
        public Player enemy;
        public Card opponentCard;
        public Action<DiscardPile> cardSelect;
        public List<GameObject> cardList = new List<GameObject>();
        public override bool Play(Player player, out string message)
        {
  
      //      Card target = enemy.discardPile.GetCard(enemy.discardPile.GetDeckSize() - 1);
           player.MoveCamera(true);
           player.playerInteract.SetInteractState(PlayerInteract.InteractState.DiscardPile);
           ShowLastCards(player);
           cardSelect += PlayLatestCard;
           message = "Please select one of the cards that you want to play";
           return true;
        }

        private void ShowLastCards(Player cardOwner)
        {
            var players = GameManager.Instance.playerList;
            Debug.Log(players.Count);
            foreach (var player in players)
            {
                if (player.Equals(cardOwner))
                    continue;
                Card lastCard = player.discardPileClass.card;
                player.discardPileClass.GetComponent<Interact>().InteractableWith.Add(cardOwner.gameObject);
                if (lastCard == null) continue;
                
                lastCard.gameObject.SetActive(true);
                
                lastCard.gameObject.transform.position = player.discardPile.transform.position + Vector3.up*4;
                lastCard.gameObject.GetComponent<Collider>().enabled = true;
                lastCard.isActive = true;
                cardList.Add(lastCard.gameObject);
            }
        }

        private void PlayLatestCard(DiscardPile discardPile)
        {
            foreach (var card in cardList)
            {
                card.transform.position = Vector3.zero;
                card.GetComponent<Collider>().enabled = true;
                card.SetActive(false);
            }
            cardSelect -= PlayLatestCard;
            GameManager.Instance.EndTurn();
        }


    }
}
