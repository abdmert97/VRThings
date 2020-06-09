using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    public enum ResourceType
    { 
        Colonist = 0,
        Water = 1,
        Food = 2,
        Wood = 3,
        Stone = 4,
        Iron = 5,
        Gold = 6
    }
    public class Resource 
    {
        public ResourceType resourceType;
        public int amount;
        public Resource(ResourceType resourceType,int amount)
        {
            this.resourceType = resourceType;
            this.amount = amount;
        }
    }

    
}