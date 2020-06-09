using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ComplexGame.Cards
{
    public class Reproduction : Card
    {
        public override bool Play(Player player, out string message)
        {
            var colonistOptionImage = GameManager.Instance.gameData.ReproductionCardOption1Image;
            var colonistOptionText = GameManager.Instance.gameData.ReproductionCardOption1Text;
            var goldOptionImage = GameManager.Instance.gameData.ReproductionCardOption2Image;
            var goldOptionText = GameManager.Instance.gameData.ReproductionCardOption2Text;
            player.playerInteract.SetInteractState(PlayerInteract.InteractState.NotInteract);
            InGameUIManager.Instance.OpenCardOptionPanel(colonistOptionImage,
                colonistOptionText, goldOptionImage, goldOptionText);
            InGameUIManager.Instance.OpenResourcePanel(player.storage);
            message = "Card play successful";
            player.discardPile.AddCard(this);
            player.deck.RemoveCard(this);
            return true;
        }

        public bool PlayOption1(Player player, out string message)
        {
            if (player.storage.ColonistAmount > 0 && player.storage.FoodAmount > 0 && player.storage.WoodAmount > 0)
            {
                var resourcesToRemove = new Dictionary<ResourceType, int>
                {
                    {ResourceType.Colonist, 1}, {ResourceType.Food, 1}, {ResourceType.Wood, 1}
                };
                player.storage.RemoveResource(resourcesToRemove, out var result);
                InGameUIManager.Instance.OpenResourcePanel(player.storage);
                Debug.Log(result);
                    
                player.MoveCamera(true);
                    
                player.playerInteract.interactState = PlayerInteract.InteractState.Vertex;
            }
            else
            {
                message = "You don't have enough resources";
                return false;
            }
            InGameUIManager.Instance.CloseCardOptionPanel();
            player.playerInteract.interactState = PlayerInteract.InteractState.Vertex;
            message = "Option 1 is chosen, please place your explorer";
            return true;
        }
        
        public bool PlayOption2(Player player, out string message)
        {
            Debug.Log("option2 chosen");
            InGameUIManager.Instance.CloseCardOptionPanel();
            var goldGain = 5 + player.explorerCount * GameManager.Instance.gameData.ReproductionCardGoldPerExplorer;
            player.storage.GoldAmount += goldGain;
            message = "Option 2 is chosen, you gained " + goldGain + " golds";
            return true;
        }
    }
}