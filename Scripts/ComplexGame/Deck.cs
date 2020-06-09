using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ComplexGame
{
    public enum DeckOrder {Cost, Type}

    public class Deck :MonoBehaviour
    {
        public List<Card> cards;
        private Transform _parent;
        private Vector3 _oldDeckPosition;
        private Quaternion _oldDeckRotation;
        public void Awake()
        {
            cards = new List<Card>();
        }
        public void AddCard(Card card)
        {
    
            cards.Add(card);
        }

        public void SetDeckTransform(Vector3 position, Quaternion rotation)
        {
            _oldDeckPosition = transform.localPosition;
            _oldDeckRotation = transform.localRotation;
            transform.localPosition = position;
            transform.localRotation = rotation;
            
        }

        public void ResetDeckTransform()
        {
            transform.localPosition = _oldDeckPosition;
            transform.localRotation = _oldDeckRotation;
        }
        public Card GetCard(int index)
        {
            return cards[index];
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        public int GetDeckSize()
        {
            return cards.Count;
        }

        public List<Card> GetCardsWithType(CardType cardType)
        {
            List<Card> cardsOfType = new List<Card>();
            foreach(Card card in cards)
            {
                if(card.cardType == cardType)
                {
                    cardsOfType.Add(card);
                }
            }
            return cardsOfType;
        }

        public bool İsContainingCardWithType(CardType cardType)
        {
            foreach (Card card in cards)
            {
                if (card.cardType == cardType)
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowDeck( Vector3 startPos, int seperator = 9)
        {
           
            GatherDeck(startPos);
            GameObject boundaryObject = cards[0].gameObject;
            Transform parent = boundaryObject.transform.parent;
            boundaryObject.transform.SetParent(null);
            Quaternion angles = boundaryObject.transform.rotation;
            boundaryObject.transform.rotation = Quaternion.identity;
            float boundX = cards[0].GetComponent<Renderer>().bounds.size.x;
            boundaryObject.transform.rotation = angles;
            boundaryObject.transform.SetParent(parent);
            
            
            float xLeft = -1 * boundX* (seperator / 2f);
            float zPoint = 0;
            int offset = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == seperator)
                {
                    zPoint = 4;
                    offset = seperator;
                }

               
                cards[i].transform.localPosition += xLeft*Vector3.right+ (boundX+0.5f)*(i - offset)*Vector3.right+Vector3.back*zPoint;
            }
  
        }
        // Organizes deck according to order given
        // direction -> -1 ascending, 1 descending
        public void OrganizeDeck(DeckOrder deckOrder, int direction)
        {
            if(deckOrder == DeckOrder.Cost)
            {
                if(direction == -1)
                {
                    cards.Sort((x, y) => x.cost.CompareTo(y.cost));
                }
                else
                {
                    cards.Sort((x, y) => y.cost.CompareTo(x.cost));
                }
            }
            else if (deckOrder == DeckOrder.Type)
            {
                // TODO: Implement
            }
        }

        // Gathers cards to form a straight deck in given position
        public void GatherDeck(Vector3 position)
        {
            foreach(Card card in cards)
            {
                card.transform.localPosition = position;
            }
        }

        public void RemoveAllCards()
        {
            cards.Clear();
        }
    }
}