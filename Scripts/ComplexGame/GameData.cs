using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameDatas", order = 55)]
    public class GameData : ScriptableObject
    {
        [SerializeField] int playerCount;
        [SerializeField] int defaultMoveCapacity;
        [SerializeField] int deckSize;
        [SerializeField] int resourceCapacity;
        [SerializeField] int resourceCount;
        [SerializeField] List<int> defaultResources;
        [SerializeField] List<int> defaultResourceGoldCosts;
        [SerializeField] float playerHeight;
        [SerializeField] GameObject explorerInteractPrefab;
        [Header("Gathering Card")]
        [SerializeField] Sprite gatheringCardOption1Image;
        [SerializeField] string gatheringCardOption1Text;
        [SerializeField] Sprite gatheringCardOption2Image;
        [SerializeField] string gatheringCardOption2Text;
        [Header("Reproduction Card")]
        [SerializeField] int reproductionCardGoldPerExplorer;
        [SerializeField] Sprite reproductionCardOption1Image;
        [SerializeField] string reproductionCardOption1Text;
        [SerializeField] Sprite reproductionCardOption2Image;
        [SerializeField] string reproductionCardOption2Text;
        [Header("Trade Card")]
        [SerializeField] int tradeCardGoldGain;

        public GameObject ExplorerInteractPrefab
        {
            get
            {
                return explorerInteractPrefab;
            }
        }
        public int PlayerCount
        {
            get
            {
                return playerCount;
            }
        }
        public int DefaultMoveCapacity
        {
            get
            {
                return defaultMoveCapacity;
            }
        }
        public float PlayerHeight
        {
            get
            {
                return playerHeight;
            }
        }
        public int DeckSize
        {
            get
            {
                return deckSize;
            }
        }
        public int ResourceCapacity
        {
            get
            {
                return resourceCapacity;
            }
        }
        public int ResourceCount
        {
            get
            {
                return resourceCount;
            }
        }
        public Sprite GatheringCardOption1Image
        {
            get
            {
                return gatheringCardOption1Image;
            }
        }
        public string GatheringCardOption1Text
        {
            get
            {
                return gatheringCardOption1Text;
            }
        }
        public Sprite GatheringCardOption2Image
        {
            get
            {
                return gatheringCardOption2Image;
            }
        }
        public string GatheringCardOption2Text
        {
            get
            {
                return gatheringCardOption2Text;
            }
        }
        
        public int ReproductionCardGoldPerExplorer
        {
            get
            {
                return reproductionCardGoldPerExplorer;
            }
        }
        public Sprite ReproductionCardOption1Image
        {
            get
            {
                return reproductionCardOption1Image;
            }
        }
        public string ReproductionCardOption1Text
        {
            get
            {
                return reproductionCardOption1Text;
            }
        }
        public Sprite ReproductionCardOption2Image
        {
            get
            {
                return reproductionCardOption2Image;
            }
        }
        public string ReproductionCardOption2Text
        {
            get
            {
                return reproductionCardOption2Text;
            }
        }
        public int TradeCardGoldGain
        {
            get
            {
                return tradeCardGoldGain;
            }
        }
        public List<int> DefaultResources
        {
            get
            {
                return defaultResources;
            }
        }
        public List<int> DefaultResourceGoldCosts
        {
            get
            {
                return defaultResourceGoldCosts;
            }
        }
    }
}

