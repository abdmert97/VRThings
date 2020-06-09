using System;

namespace ComplexGame.Cards
{
    // TODO: Add resource space check via checking return value of addresource function
    public class Production : Card
    {
        public TileType tileTypeSelected;
        public override bool Play(Player player, out string message)
        {
            var resourceGain = 0;
            switch (tileTypeSelected)
            {
                case TileType.Lake:
                    resourceGain = GameManager.Instance.hexMap.GetCountOfVillagesWithType(TileType.Lake, player.PlayerNumber);
                    player.storage.AddResource(ResourceType.Water, resourceGain);
                    message = "Card play successful, gained " + resourceGain +  " water";
                    break;
                case TileType.Field:
                    resourceGain = GameManager.Instance.hexMap.GetCountOfVillagesWithType(TileType.Field, player.PlayerNumber);
                    player.storage.AddResource(ResourceType.Food, resourceGain);
                    message = "Card play successful, gained " + resourceGain +  " food";
                    break;
                case TileType.Forest:
                    resourceGain = GameManager.Instance.hexMap.GetCountOfVillagesWithType(TileType.Forest, player.PlayerNumber);
                    player.storage.AddResource(ResourceType.Wood, resourceGain);
                    message = "Card play successful, gained " + resourceGain +  " wood";
                    break;
                case TileType.Hill:
                    resourceGain = GameManager.Instance.hexMap.GetCountOfVillagesWithType(TileType.Hill, player.PlayerNumber);
                    player.storage.AddResource(ResourceType.Stone, resourceGain);
                    message = "Card play successful, gained " + resourceGain +  " stone";
                    break;
                case TileType.Mountain:
                    resourceGain = GameManager.Instance.hexMap.GetCountOfVillagesWithType(TileType.Mountain, player.PlayerNumber);
                    player.storage.AddResource(ResourceType.Iron, resourceGain);
                    message = "Card play successful, gained " + resourceGain +  " iron";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            player.discardPile.AddCard(this);
            player.deck.RemoveCard(this);
            return true;
        }

     
    }
}
