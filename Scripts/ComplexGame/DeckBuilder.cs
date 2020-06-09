using System.Collections.Generic;
using ComplexGame.Cards;
using TMPro;
using UnityEngine;

namespace ComplexGame
{
    public class DeckBuilder : MonoBehaviour
    {
        public int deckSize;
        public CardData cardData;
        public GameObject discardDeckprefab;
        public GameObject discardPileActivationEffect;
        public GameObject playCardEffect;
        public static Vector3 defaultCardScale = new Vector3(1.35f, 2, 0.1f)*1.5f;
        private void Awake()
        {
            cardData = Instantiate(cardData);
            deckSize = GameManager.Instance.gameData.DeckSize;
    
        }
    

        public GameObject GetDiscardPile()
        {
            Vector3 cardScale = new Vector3(1.35f, 2, 0.2f)*5f ;
            Vector3 realScale = BoundCalculator.SetUnitScale(discardDeckprefab, cardScale.x, cardScale.y, cardScale.z);
            GameObject pile = Instantiate((discardDeckprefab));
           // pile.transform.localScale = realScale;
            Deck deck = pile.AddComponent<Deck>();
            DiscardPile discardPile = pile.AddComponent<DiscardPile>();
            
          
        

            return pile;
        }

        public Deck GetStarterDeck(Transform parent = null,List<GameObject> interactables= null)
        {
            //1 travel, 2 collector, 1 trade, 1 shopping, 1 theft and 1 recycle card. 
          
            Deck deck = parent.gameObject.AddComponent<Deck>();
            Vector3 cardScale = new Vector3(1.35f, 2, 0.1f)*1.5f;
            GenerateCard(CardType.Migration, parent, Vector3.zero,cardScale ,deck,interactables);
            GenerateCard(CardType.Gathering, parent, Vector3.zero,cardScale ,deck,interactables);
            GenerateCard(CardType.Gathering, parent, Vector3.zero,cardScale,deck,interactables);
            GenerateCard(CardType.Trade, parent, Vector3.zero,cardScale ,deck,interactables);
            GenerateCard(CardType.Growth, parent, Vector3.zero,cardScale,deck,interactables);
            GenerateCard(CardType.Diplomacy, parent, Vector3.zero,cardScale,deck,interactables);
            GenerateCard(CardType.NewAge, parent, Vector3.zero,cardScale ,deck,interactables);
            
            
          
            return deck;
        }
        public Card GenerateCard(CardType cardType, Deck deck = null, Transform parent = null)
        {
            GameObject card = Instantiate(cardData.CardPrefabs[(int) cardType], parent);
            if (deck != null) deck.AddCard(card.GetComponent<Card>());
            return card.GetComponent<Card>();
        }
        public Card GenerateCard(CardType cardType,  Transform parent,Vector3 position,Vector3 scale, Deck deck = null,List<GameObject>interactables = null,GameObject singleInteractable = null)
        {
            Vector3 realScale = BoundCalculator.SetUnitScale(cardData.CardPrefabs[(int) cardType], scale.x, scale.y, scale.z);
            GameObject card = Instantiate(cardData.CardPrefabs[(int) cardType], parent);
            card.tag = "Card";
            card.transform.localPosition = position;
            card.transform.localScale = realScale;
            card.transform.Rotate(Vector3.right*90);
            var cardClass = AddCardComponent(cardType, card);
            AddCardInfos(cardType, cardClass, card);

            if (deck != null) 
                deck.AddCard(card.GetComponent<Card>());
           
            AddInteraction(interactables, singleInteractable, card);
            return card.GetComponent<Card>();
        }

        private static void AddInteraction(List<GameObject> interactables, GameObject singleInteractable, GameObject card)
        {
            if (interactables != null)
            {
                Interact interact = card.GetComponent<Interact>();
                interact.interactType = Interact.InteractType.Drag;
                if (interact)
                {
                    foreach (var interactable in interactables)
                    {
                        interact.InteractableWith.Add(interactable);
                    }
                }
            }

            if (singleInteractable != null)
            {
                Interact interact = card.GetComponent<Interact>();
                interact.interactType = Interact.InteractType.Drag;
                interact.InteractableWith.Add(singleInteractable);
            }
        }

        private void AddCardInfos(CardType cardType, Card cardClass, GameObject card)
        {
            cardClass.SetInfo(cardType, cardData.Costs[(int) cardType], cardData.Titles[(int) cardType],
                cardData.Texts[(int) cardType]);


            //Frontside
            card.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite =
                cardData.FrontFaces[(int) cardType];

            //title
            card.transform.GetChild(2).GetComponent<TextMeshPro>().text = cardData.Titles[(int) cardType];

            // text
            card.transform.GetChild(3).GetComponent<TextMeshPro>().text = cardData.Texts[(int) cardType];
        }

        private static Card AddCardComponent(CardType cardType, GameObject card)
        {
            Card cardClass;
            //    public enum CardType { Migration, Gathering, Trade, Growth, Diplomacy,NewAge,Reproduction,WaterProduction,FoodProduction,WoodProduction,StoneProduction,IronProduction} 
            switch (cardType)
            {
                case CardType.Migration:
                    cardClass = card.AddComponent<Migration>();
                    break;
                case CardType.Gathering:
                    cardClass = card.AddComponent<Gathering>();
                    break;
                case CardType.Trade:
                    cardClass = card.AddComponent<Trade>();
                    break;
                case CardType.Growth:
                    cardClass = card.AddComponent<Growth>();
                    break;
                case CardType.Diplomacy:
                    cardClass = card.AddComponent<Diplomacy>();
                    break;
                case CardType.NewAge:
                    cardClass = card.AddComponent<NewAge>();
                    break;
                case CardType.Reproduction:
                    cardClass = card.AddComponent<Reproduction>();
                    break;
                default:
                    cardClass = card.AddComponent<Reproduction>();
                    break;
            }

            return cardClass;
        }
    }
}
