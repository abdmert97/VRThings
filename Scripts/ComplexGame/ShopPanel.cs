using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ComplexGame
{
    public class ShopPanel : MonoBehaviour
    {
        public GameObject shopPanel;
        public List<GameObject> cards;
        private List<Vector3> _cardPostions;
        public List<GameObject> cardPrefabs;
        public List<Card> _cardShop;
        public Stack<Card> _deck;
        private DeckBuilder _deckBuilder;
        private Random random = new Random();
        public Deck deck;
        public TextMeshPro [] prices = new TextMeshPro[6];
        private Animator _animator;
        public Transform cardParent;
        private void Start()
        {
            _deckBuilder = GameManager.Instance.deckBuilder;
            _cardShop = new List<Card>(6);
            CreateInitialShop();
            TurnManager.EndTurn += ClosePanel;
            _animator = GetComponent<Animator>();
        }

        private void CreateInitialShop()
        {
            Vector3 cardScale = new Vector3(1.35f, 2, 0.1f)*1.5f;
            for (int i = 0; i < 6 ; i++)
            {
                _deckBuilder.GenerateCard(GetRandomCardType(),cardParent,transform.position,cardScale,deck);
                deck.cards[i].GetComponent<Interact>().interactType = Interact.InteractType.Click;
                prices[i].text =  GetCost(i).ToString() + " Gold";
            }

            deck.ShowDeck(Vector3.down,6);
            gameObject.SetActive(false);
        }

     
        public void SetVisibleToPlayer(Player player)
        {
            Camera playerCamera = player.playerCamera;
            Ray ray = playerCamera.ViewportPointToRay(Vector3.one*0.5f);
            Vector3 position = ray.GetPoint(10);
            transform.position = position;
            Vector3 angles = player.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(angles-Vector3.right*70);
            AddInteractions(player);
            gameObject.SetActive(true);
        }

        private void AddInteractions(Player player)
        {
            foreach (var card in deck.cards)
            {
                card.GetComponent<Interact>().InteractableWith.Add(player.gameObject);                
            }
        }


        private CardType GetRandomCardType()
        {
            var v = Enum.GetValues (typeof(CardType));
            return (CardType) v.GetValue (Random.Range(0,v.Length));
        }
        public int GetAdditionalCost(int index)
        {
            int cost;
            cost = GetCost(index);
            return cost;
        }

        public int GetCost(int index)
        {
            int cost;
            switch (index)
            {
                case 1:
                case 2:
                    cost = 3;
                    break;
                case 3:
                case 4:
                    cost = 5;
                    break;
                case 5:
                    cost = 7;
                    break;
                default:
                    cost = 0;
                    break;
            }

            return cost;
        }

        public int GetAdditionalCost(Card card)
        {
            int index = IndexOfCard(card);

            return GetCost(index);
        }

        public int GetAdditionalCost(List<Card> cardList)
        {
            int totalCost = 0;
            foreach(var card in cardList)
            {
                totalCost += GetAdditionalCost(card);
            }

            return totalCost;
        }

        public void OpenPanel()
        {
            _animator.Play("OpenShop");
            shopPanel.SetActive(true);
        }
        public void ClosePanel()
        {
            shopPanel.SetActive(false);
        }
        public void ShowDeck()
        {
            for (int i = 0; i <cards.Count; i++)
            {
                cards[i].SetActive(true);
                cards[i].transform.localPosition = _cardPostions[i];
            }
        }


        public int IndexOfCard(Card card)
        {
            for(int i = 0; i < deck.cards.Count; i++)
            {
                if(card.Equals(deck.cards[i]))
                {
                    return i;
                }
            }
            return -1;
        }


        public void RemoveCard(Card card)
        {    
           Vector3 cardScale = new Vector3(1.35f, 2, 0.1f)*1.5f;
           deck.cards.Remove(card);
           Destroy(card.gameObject);
           Card generated =  _deckBuilder.GenerateCard(GetRandomCardType(),cardParent,transform.position,cardScale,deck);
           generated.GetComponent<Interact>().interactType = Interact.InteractType.Click;
           deck.ShowDeck(Vector3.zero,6);
        }



/*        public bool IsPlayerHasEnoughResources(Player player, Card card)
        {
            for(int i = 0; i < card.resourceCost.Count; i++)
            {
                if (!player.storehouse.IsThereResource(card.resourceCost[i].resourceType))
                {
                    return false;
                }
            }

            return true;
        }*/




/*    public void BuyCard(GameObject card)
    {
        cards.Remove(card);
        GetCardFromDeck();
        
    }





    private void GetCardFromDeck()
    {
        GameObject newCard = Instantiate(cardPrefabs[UnityEngine.Random.Range(0,cardPrefabs.Count)], transform);
        ShowDeck();
    }*/
    }
}
