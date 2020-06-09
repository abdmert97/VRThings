using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardDatas", order = 56)]
    public class CardData : ScriptableObject
    {
        [SerializeField] List<GameObject> cardPrefabs;
        [SerializeField] List<string> titles;
        [SerializeField] List<string> texts;
        [SerializeField] List<Sprite> frontFaces;
        [SerializeField] private List<int> costs;
        public List<GameObject> CardPrefabs
        {
            get
            {
                return cardPrefabs;
            }
        }
        public List<string> Titles
        {
            get
            {
                return titles;
            }
        }
        public List<string> Texts
        {
            get
            {
                return texts;
            }
        }
        public List<Sprite> FrontFaces
        {
            get
            {
                return frontFaces;
            }
        }
        public List<int> Costs
        {
            get
            {
                return costs;
            }
        }

 
    }
}
