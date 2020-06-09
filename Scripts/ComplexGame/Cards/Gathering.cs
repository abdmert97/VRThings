using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame.Cards
{
    public class Gathering : Card
    {
        public  Action<Player,int> regionSelected;
        public override bool Play(Player player, out string message)
        {
            var activateOptionImage = GameManager.Instance.gameData.GatheringCardOption1Image;
            var activateOptionText = GameManager.Instance.gameData.GatheringCardOption1Text;
            var deactivateOptionImage = GameManager.Instance.gameData.GatheringCardOption2Image;
            var deactivateOptionText = GameManager.Instance.gameData.GatheringCardOption2Text;
            player.playerInteract.SetInteractState(PlayerInteract.InteractState.NotInteract);
            InGameUIManager.Instance.OpenCardOptionPanel(activateOptionImage,
                activateOptionText, deactivateOptionImage, deactivateOptionText);
            message = "Card play successful";
            player.discardPile.AddCard(this);
            player.deck.RemoveCard(this);
            return true;
        }
        public bool PlayOption1(Player player, out string message)
        {
            Debug.Log("option1 chosen");
            InGameUIManager.Instance.CloseCardOptionPanel();
            player.MoveCamera(true);
            player.playerInteract.SetInteractState(PlayerInteract.InteractState.Region);
            // TODO: Handle get region coordinate
            regionSelected+= ActivateRegion;
            message = "Waiting to select region";
            return true;
        }

        private void ActivateRegion(Player player,int regionID)
        {
            string message;
            if (!BuildManager.Instance.ActivateRegion(regionID, GameManager.Instance.GetCurrentPlayerNumber(),
                out Dictionary<ResourceType, int> resources, out message))
                return;
            if (!player.storage.AddResource(resources, out message))
                return;

            regionSelected -= ActivateRegion;
            Debug.Log("Region activated "+ regionID);
            GameManager.Instance.EndTurn();
            
            
        }

        public bool PlayOption2(Player player, out string message)
        {
            Debug.Log("option2 chosen");
            InGameUIManager.Instance.CloseCardOptionPanel();
            
            var goldGain = BuildManager.Instance.DeactivateRegions();
            player.storage.AddResource(ResourceType.Gold, goldGain);
            Debug.Log(goldGain +" gold is added");
            GameManager.Instance.EndTurn();
            
            message = "Option 2 is chosen, gained " + goldGain + " golds";
            return true;
        }
    }
}
