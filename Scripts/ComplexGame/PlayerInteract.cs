using System.Collections;
using System.Collections.Generic;
using ComplexGame.Cards;
using UnityEngine.UI;
using UnityEngine;

namespace ComplexGame
{
    public class PlayerInteract
    {
        public enum InteractState  {Card,Hex,Vertex,HexAndExplorer,Explorer,NotInteract,Region,DiscardPile}
        public GameObject latestInteractedObject;
        public Player player;
        public Hexmap hexmap;
        public InteractState previousInteractState;
        public InteractState interactState;
        public int moveCapacity;
        public ParticleSystem explorerParticle;
        public void SetMoveCapacity()
        {
            moveCapacity = GameManager.Instance.gameData.DefaultMoveCapacity;
        }

        private void SetStartState()
        {
            interactState = InteractState.Card;
        }

        public void SetInteractState(InteractState interactState)
        {
            this.previousInteractState = interactState;
            this.interactState = interactState;
        }
        public void InteractWith(GameObject interactableObject,Interact.InteractType interactType)
        {
            Debug.Log(interactableObject.name + " start interact with " + interactableObject.tag);
            if (interactableObject.CompareTag("Card")&& interactState == InteractState.Card)
            {
                CardInteraction(interactableObject);
            }
            else if (interactableObject.CompareTag("Hex") && (interactState == InteractState.Hex ||interactState == InteractState.HexAndExplorer))
            {
                HexInteraction(interactableObject);
            }
            else if (interactableObject.CompareTag("Hex") && interactState == InteractState.Region)
            {
                RegionInteraction(interactableObject);
            }
            else if (interactableObject.CompareTag("Vertice") && interactState == InteractState.Vertex)
            {
                VertexInteraction(interactableObject);
            }
            else if (interactableObject.CompareTag("Explorer")&& (interactState == InteractState.Explorer || interactState == InteractState.HexAndExplorer ))
            {
                ExplorerInteraction(interactableObject);
            }
            else if (interactState == InteractState.NotInteract)
            {
                NotInteract(interactableObject);
            }
            else if (interactableObject.CompareTag("PileDeck")&&interactState == InteractState.DiscardPile)
            {
                SelectDiscardPile(interactableObject);
            }

            if (!interactableObject.CompareTag("Explorer"))
            {
                StopExplorerParticle();
            }
        }

        private void SelectDiscardPile(GameObject interactableObject)
        {
            if (player.discardPileClass.card.cardType == CardType.Diplomacy)
            {
                DiscardPile discardPile = interactableObject.GetComponent<DiscardPile>();
                player.discardPileClass.card.GetComponent<Diplomacy>().cardSelect.Invoke(discardPile);
                discardPile.GetComponent<Interact>().InteractableWith.Remove(player.gameObject);
            }
        }

        private void RegionInteraction(GameObject interactableObject)
        {
            int regionID = hexmap.GetRegionCoordinate(interactableObject);
            if (player.discardPileClass.card.cardType == CardType.Gathering)
            { 
                player.discardPileClass.card.GetComponent<Gathering>().regionSelected.Invoke(player, regionID);
            }
        }

        private static void NotInteract(GameObject interactableObject)
        {
            string result = "Fail";
            if (interactableObject.CompareTag("Hex"))
            {
                result = " You cannot build house now.";
            }
            else if (interactableObject.CompareTag("Explorer"))
            {
                result = " You cannot move explorer now.";
            }
            else if (interactableObject.CompareTag("Vertice"))
            {
                result = " You can not place explorer now.";
            }

            AnimatedCanvas.Instance.ShowError(result);
        }

        private void ExplorerInteraction(GameObject interactableObject)
        {
            SelectExplorer(interactableObject);
            PlayExplorerEffect(interactableObject.transform);
            SetInteractState(InteractState.Vertex);
        }

        private void VertexInteraction(GameObject interactableObject)
        {
            int index = hexmap.GetVertexCoordinate(interactableObject);

            string result;
            if (latestInteractedObject != null && latestInteractedObject.CompareTag("Explorer"))
            {
                SetInteractState(InteractState.HexAndExplorer);
                MoveExplorer(interactableObject);
            }
            else if (player.items.transform.childCount != 0)
            {
                var explorerPrefab = player.items.transform.GetChild(0).gameObject;
                bool success = hexmap.PlaceExplorer(index, player.PlayerNumber, explorerPrefab,
                    out result);
                if (success)
                {
                    player.explorerCount++;
                    player.activeExplorers.Add(explorerPrefab);
                    SetInteractState(InteractState.NotInteract);
                }

                if (success)
                {
                    AnimatedCanvas.Instance.ShowMessage(result);
                }
                else
                {
                    AnimatedCanvas.Instance.ShowError(result);
                }
            }
        }

     

        private void HexInteraction(GameObject interactableObject)
        {
            Hex hex = hexmap.GetHexCoordinate(interactableObject);

            BuildHouse(hex);
        }

        private void CardInteraction(GameObject interactableObject)
        {
            Card card = interactableObject.GetComponent<Card>();
            if (card != null)
                OpenInfoPanel(card);
            if (card.GetComponent<Interact>().interactType == Interact.InteractType.Click) // Shop Card
            {
                ShopInteraction(card);
            } 
            else
            {
                player.ActivateDiscardPile.Invoke();
            }
        
        }

        private void ShopInteraction(Card card)
        {
            ShopPanel shopPanel = GameManager.Instance.shopPanel;
            int index = shopPanel.IndexOfCard(card);
            int cost = shopPanel.GetAdditionalCost(index);
            if (player.storehouse.IsThereEnoughGold(cost))
            {
                player.storehouse.SpendGold(cost);
                shopPanel.RemoveCard(card);
                GameManager.Instance.deckBuilder.GenerateCard(card.cardType, player.deck.transform,
                    Vector3.zero,DeckBuilder.defaultCardScale, player.deck, null, player.gameObject);
                player.deck.ShowDeck(Vector3.down * 0.65f);
              shopPanel.ClosePanel();
               GameManager.Instance.EndTurn();
            }
            else
            {
                AnimatedCanvas.Instance.ShowError("You don't have enough gold to buy this card");
            }
        }

        private void PlayExplorerEffect(Transform interactableObjectTransform)
        {
            Vector3 position = interactableObjectTransform.position+Vector3.up*0.2f;
            
            if (explorerParticle == null)
            {
                explorerParticle = player.CreateParticle(GameManager.Instance.gameData.ExplorerInteractPrefab);
     
                explorerParticle.transform.SetParent(player.transform);
              
            }
            explorerParticle.gameObject.transform.position = position;
            explorerParticle.Play();
        }

        public void EndInteractWith(GameObject interactableObject,Interact.InteractType interactType)
        {
          //  Debug.Log(interactableObject.name+"end interact with" + name);
            if (interactableObject.CompareTag("Card") && interactState == InteractState.Card)
            {
                player.DeActivateDiscardPile.Invoke();
                Card card = interactableObject.GetComponent<Card>();
                CloseInfoPanel();
            }
        }

        void StopExplorerParticle()
        {
            if (explorerParticle && explorerParticle.isPlaying)
            {
                explorerParticle.Stop();
            }
        }
        
        public void SetPlayer(Player player)
        {
            this.player = player;
            player.EndTurn += SetStartState;
            player.EndTurn+=ResetLatestInteract;
            player.EndTurn += SetMoveCapacity;
            moveCapacity = 6;
            
        }
        public void OpenInfoPanel(Card card)
        {
        
            Sprite frontFace = GameManager.Instance.deckBuilder.cardData.FrontFaces[(int) card.cardType];
            InGameUIManager.Instance.OpenInfoPanel(frontFace,card.cardTitle,card.cardInfo);
        }

        public void CloseInfoPanel()
        {
            InGameUIManager.Instance.CloseInfoPanel();
        }

        public void BuildHouse(Hex hex)
        {
            BuildManager.Instance.BuildHouse(player.PlayerNumber,hex);
        }

        public void SelectExplorer(GameObject explorer)
        {
            latestInteractedObject = explorer;
        }

        public void MoveExplorer(GameObject vertice)
        {
            Hexmap hexmap = GameManager.Instance.hexMap;
            int from;
            bool success = int.TryParse(latestInteractedObject.name,out from);
            if (success)
            {
                int to = hexmap.GetVertexCoordinate(vertice);
                string result;
                
                success = hexmap.MoveExplorer(player,latestInteractedObject.GetComponent<Animator>(),from, to, player.PlayerNumber, out result);
                latestInteractedObject = null;
                if (success)
                {
                    AnimatedCanvas.Instance.ShowMessage(result);
                }
                else
                {
                    AnimatedCanvas.Instance.ShowError(result);
                }
            }
        }

        public void ResetLatestInteract()
        {
            latestInteractedObject = null;
        }
    }


}
