using System.Collections.Generic;

namespace ComplexGame.Cards
{
    public class Growth : Card
    {
        // TODO: player should choose card actions
        public List<Card> selectedCards;
        public override bool Play(Player player, out string message)
        {

            GameManager.Instance.shopPanel.SetVisibleToPlayer(player);
            // TODO: Wait implementation on Shop side
          /*  Storehouse store = player.storehouse;
            ShopPanel shop = GameManager.Instance.shopPanel;
            if (store.IsThereEnoughResourceForMultipleCards(selectedCards) && store.IsThereEnoughGold(shop.GetAdditionalCost(selectedCards)))
            {
                //Removed storage from storehouse
                foreach (var card in selectedCards)
                {
                    store.RemoveResourcesFromList(card.resourceCost);
                }

                //Get Cards to players hand
                foreach (var card in selectedCards)
                {
                    player.deck.AddCard(card);
                }

                //RemoveCards from shop
                shop.RemoveMultipleCardsFromShop(selectedCards);

                message = "Successfully Purchased";
                player.discardPile.AddCard(this);
                player.deck.RemoveCard(this);
                return true;
            }
            else
            {
                message = "Are you sure you have enough storage?";
                return false;
            }*/
          message = "Are you sure you have enough storage?";
          return false;
        }
    }
}
