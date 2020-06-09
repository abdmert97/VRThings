using System;
using System.Collections;
using System.Collections.Generic;
using ComplexGame.Cards;
using UnityEngine;

namespace ComplexGame
{
    public enum CardType { Migration, Gathering, Trade, Growth, Diplomacy,NewAge,Reproduction,WaterProduction,FoodProduction,WoodProduction,StoneProduction,IronProduction,Production}
    public abstract class Card : MonoBehaviour
    {
    
        public CardType cardType;
        public bool isActive;
        public int cost;
        public string cardInfo;
        public int cardSet;
        public string cardTitle;
        
        //Added by mert ak
        public List<Resource> resourceCost;
        private void Start()
        {
            isActive = true;
            // when recycle card used automatically all card will be available
            NewAge.RecycleCards += ReActivateCard;
        }

        public void SetInfo(CardType cardType, int cost, string title, string info)
        {
            this.cardType = cardType;
            this.cost = cost;
            cardTitle = title;
            cardInfo = info;

        }

        public void ReActivateCard()
        {
            isActive = true;
            // TODO: visual implementations
        }
        public void SendDiscardPile(Transform discardPile)
        {
            // sample implementation
            transform.position = discardPile.transform.position+Vector3.up*0.2f;
        }
        public abstract bool Play(Player player,out string message);


    }    
}