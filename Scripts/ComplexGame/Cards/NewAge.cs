using UnityEngine;

namespace ComplexGame.Cards
{
    public class NewAge : Card
    {

        public static System.Action RecycleCards;
        
        private Player _player;
        // TODO recycling cards will recycle this card too
        public override bool Play(Player player, out string message)
        {
            
            RecycleCards.Invoke();
            // Add cards in discard pile to players deck
            // TODO: testing
            player.storage.AddResource(ResourceType.Gold, (int) Mathf.Ceil((float) player.discardPile.cards.Count / 3));
            _player = player;
            Invoke(nameof(ReactivateCards),2f);
            if (player.storage.BuyResource(ResourceType.Colonist, 1, out message) == false)
            {
                Debug.Log(message);
                GameManager.Instance.EndTurn();
                return false;
            }
            else
            {
                Debug.Log(message);
                player.MoveCamera(true);
                player.playerInteract.SetInteractState(PlayerInteract.InteractState.Vertex);
            }
            player.discardPile.AddCard(this);
            player.deck.RemoveCard(this);
            
            
            message = "Card played successfully";
            return true;
        }
        private void ReactivateCards()
        {
            foreach (var discardedCard in _player.discardPileClass.cards)
            {
                discardedCard.gameObject.SetActive(true);
                discardedCard.isActive = true;
                _player.deck.cards.Add(discardedCard);
            }
            _player.discardPileClass.cards.Clear();
                
            _player.deck.ShowDeck(Vector3.down * 0.65f);
        }
    }
   
}
