using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ComplexGame
{
    public class DiscardPile : MonoBehaviour
    {

        public Player player;
        public ParticleSystem activateEffect;
        public ParticleSystem playEffect;
        public Card card;
        
        public Deck discardDeck;
        public Interact discardPileInteract;
        public List<Card> cards;
        public float highlightedRateOverTime = 100;
        public float unhighlightedRateOverTime = 18;
        // Activate Effect Settings
       
        private readonly Color _highlightedGlowColor = new Color(0, 96, 255, 0.25f);
        private readonly Color _unhighlightedGlowColor = new Color(0, 96, 255, 0.1f);
        private Interact _interact;
        private void Awake()
        {
            discardPileInteract = gameObject.GetComponent<Interact>();
            
           
        }

        void Start()
        {
            player.ActivateDiscardPile+= ActivateDiscardPile;
            player.DeActivateDiscardPile+= DeActivateDiscardPile;
            cards = new List<Card>();
            AddInteraction(player.gameObject);
            //player.Play += HighlightDiscardPile;
            //player.EndTurn += UnhighlightDiscardPile;

           
        }

        public void AddInteraction(GameObject InteractablePlayer)
        {
           discardPileInteract.InteractableWith.Add(InteractablePlayer);
        }
        private void DeActivateDiscardPile()
        {
            activateEffect.Stop();
        }
        private void ActivateDiscardPile()
        {
            if (activateEffect == null)
            {
                CreateDiscardPileEffect();
            }
            var emission = activateEffect.emission;
            emission.rateOverTime = unhighlightedRateOverTime;
            SetGlowColor(_unhighlightedGlowColor);
            activateEffect.Play();
        }

        private void CreateDiscardPileEffect()
        {
            GameObject particle = Instantiate(GameManager.Instance.deckBuilder.discardPileActivationEffect, transform, true);
            particle.transform.localPosition = Vector3.zero;
            activateEffect = particle.GetComponent<ParticleSystem>();
            particle.transform.rotation = transform.rotation;
            particle.transform.Rotate(0, 180, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Card"))
            {
                card = other.GetComponent<Card>();
                _interact = other.GetComponent<Interact>();
                Debug.Log("Card Selected");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Card"))
            {
                card = null;
                _interact = null;
                var emission = activateEffect.emission;
                emission.rateOverTime = unhighlightedRateOverTime;
                SetGlowColor(_unhighlightedGlowColor);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (card && _interact.isReleased)
            {
                // Card Played
                _interact.isEnabled = true;
                discardDeck.AddCard(card);
               
                player.deck.cards.Remove(card);
                if (playEffect == null)
                {
                    CreatePlayEffect();
                }
                playEffect.Play();
                Invoke(nameof(StopPlayEffect),1f);
                String played ="CardPlayed";
                InGameUIManager.Instance.CloseResourcePanel();
                played = PlayCard();
                ShowTileEffect();
                AnimatedCanvas.Instance.ShowMessage(played);
               
            }
            else if(card && !_interact.isReleased  && activateEffect!=null)
            {
                var emission = activateEffect.emission;
                emission.rateOverTime = highlightedRateOverTime;
                SetGlowColor(_highlightedGlowColor);
            }
        }

        private void ShowTileEffect()
        {
            GameManager.Instance.OpenTileEffect();
            GameManager.Instance.OpenVertexEffect();
        }

        private string PlayCard()
        {
            string played;
            card.isActive = false;
            card.gameObject.SetActive(false);
            cards.Add(card);
            card.GetComponent<Interact>().isEnabled = false;
            card.Play(player, out played);
            return played;
        }

        private void CreatePlayEffect()
        {
            GameObject cardEffect = Instantiate(GameManager.Instance.deckBuilder.playCardEffect, transform, true);
            playEffect = cardEffect.GetComponent<ParticleSystem>();
            cardEffect.transform.position = transform.position;
        }

        private void StopPlayEffect()
        {
            playEffect.Stop();
        }

        private void SetGlowColor(Color color)
        {
            var glow = activateEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            glow.startColor = color;
        }
    }

}

