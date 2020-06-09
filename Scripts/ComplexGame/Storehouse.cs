using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    public class Storehouse : MonoBehaviour 
    {
       // public GameObject storeHouse;
        public int golds;
        public int maxLimit = 12;
        public List<Resource> resources;
        

        public void Awake()
        {
            maxLimit = 12; // by default
            resources  = new List<Resource>();
        }

        public int GetResourceAmount(ResourceType type)
        {
            return resources[(int) type].amount;
        }

        public void RemoveResource(ResourceType type, int amount)
        {
            resources[(int) type].amount -= amount;
        }

        public void AddGold(int amount)
        {
            golds += amount;
        }

        public void SpendGold(int amount)
        {
            golds -= amount;
        }

        public bool IsThereEnoughGold(int amount)
        {
            if(golds >= amount)
            {
                return true;
            }

            return false;
        }

        public void RemoveResourcesFromList(List<Resource> resourceList)
        {
            foreach(var resource in resourceList)
            {
                resources.Remove(resource);
            }
        }

        public bool AddResource(ResourceType type, int amount)
        {
            if(resources.Count + amount > maxLimit)
            {
                return false;
            }

            for(int i = 0; i < amount; i++)
            {
                Resource tmp = new Resource(type,amount);
                resources.Add(tmp);
            }

            return true;
        }

        public void SetDefaultResources()
        {
            List<int> resourceAmount = GameManager.Instance.gameData.DefaultResources;
            
            for (int i = 0; i < resourceAmount.Count; i++)
            {
                resources.Add(new Resource((ResourceType)i, resourceAmount[i]));
            }

        }

        public bool IsThereEnoughResource(ResourceType type, int amount)
        {
            if(GetResourceAmount(type) >= amount)
            {
                return true;
            }

            return false;
        }

        public bool IsThereEnoughResourceForMultipleCards(List<Card> cards)
        {
            Dictionary<ResourceType, int> resourceMap = new Dictionary<ResourceType, int>();
            for(int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                for (int j = 0; j < card.resourceCost.Count; j++)
                {
                    ResourceType resourceType = card.resourceCost[j].resourceType;
                    if (resourceMap.ContainsKey(resourceType))
                    {
                        resourceMap[resourceType]++;
                    } else
                    {
                        resourceMap.Add(resourceType, 0);
                    }
                    
                }
            }

            foreach (var pair in resourceMap)
            {
                ResourceType resource = pair.Key;
                int amount = pair.Value;
                if(!IsThereEnoughResource(resource, amount))
                {
                    return false;
                }
            }

            return true;
        }


        public int GetAvailableSpace()
        {
            return maxLimit - resources.Count;
        }
    }
}
