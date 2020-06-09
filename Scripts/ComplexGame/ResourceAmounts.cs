using System;
using System.Collections.Generic;
using System.Diagnostics;
using ComplexGame;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;


// Denotes an amount consisted of different storage
// TODO: Should be used in Card costs, Player storage, Gathered resource amounts...
namespace ComplexGame
{
    public class ResourceAmounts
    {
        #region Privates
        // Amount limit is the limit of items(except gold) an instance can hold
        public int AmountLimit { get; set; }

        // Amounts of items in the instance
        public int ColonistAmount { get; set; }
        public int FoodAmount { get; set; }
        public int WaterAmount { get; set; }
        public int IronAmount { get; set; }
        public int WoodAmount { get; set; }
        public int StoneAmount { get; set; }
        public int GoldAmount { get; set; }
        #endregion

        public ResourceAmounts()
        {
            this.AmountLimit = int.MaxValue;
            this.ColonistAmount = 0;
            this.WaterAmount = 0;
            this.FoodAmount = 0;
            this.IronAmount = 0;
            this.WoodAmount = 0;
            this.StoneAmount = 0;
            this.GoldAmount = 0;
        }

        public ResourceAmounts(int colonistAmount, int waterAmount, int foodAmount, 
            int ironAmount, int woodAmount, int stoneAmount, int goldAmount, int amountLimit = int.MaxValue)
        {
            this.AmountLimit = amountLimit;
            this.ColonistAmount = colonistAmount;
            this.WaterAmount = waterAmount;
            this.FoodAmount = foodAmount;
            this.IronAmount = ironAmount;
            this.WoodAmount = woodAmount;
            this.StoneAmount = stoneAmount;
            this.GoldAmount = goldAmount;
        }

        #region OperatorOverloading
        // Does not include _amountLimit
        public static ResourceAmounts operator -(ResourceAmounts resourceAmounts)
        {
            resourceAmounts.ColonistAmount = -resourceAmounts.ColonistAmount;
            resourceAmounts.FoodAmount = -resourceAmounts.FoodAmount;
            resourceAmounts.WaterAmount = -resourceAmounts.WaterAmount;
            resourceAmounts.IronAmount = -resourceAmounts.IronAmount;
            resourceAmounts.WoodAmount = -resourceAmounts.WoodAmount;
            resourceAmounts.StoneAmount = -resourceAmounts.StoneAmount;
            resourceAmounts.GoldAmount = -resourceAmounts.GoldAmount;
            return resourceAmounts;
        }

        // Does not include _amountLimit
        public static ResourceAmounts operator +(ResourceAmounts resourceAmounts1,
            ResourceAmounts resourceAmounts2)
        {
            ResourceAmounts resourceValues3 = new ResourceAmounts(
                resourceAmounts1.ColonistAmount + resourceAmounts2.ColonistAmount,
                resourceAmounts1.FoodAmount + resourceAmounts2.FoodAmount,
                resourceAmounts1.WaterAmount + resourceAmounts2.WaterAmount,
                resourceAmounts1.IronAmount + resourceAmounts2.IronAmount,
                resourceAmounts1.WoodAmount + resourceAmounts2.WoodAmount,
                resourceAmounts1.StoneAmount + resourceAmounts2.StoneAmount,
                resourceAmounts1.GoldAmount + resourceAmounts2.GoldAmount);

            return resourceValues3;

        }

        // Does not include _amountLimit
        public static ResourceAmounts operator -(ResourceAmounts resourceAmounts1,
            ResourceAmounts resourceAmounts2)
        {
            ResourceAmounts resourceValues3 = new ResourceAmounts(
                resourceAmounts1.ColonistAmount - resourceAmounts2.ColonistAmount,
                resourceAmounts1.FoodAmount - resourceAmounts2.FoodAmount,
                resourceAmounts1.WaterAmount - resourceAmounts2.WaterAmount,
                resourceAmounts1.IronAmount - resourceAmounts2.IronAmount,
                resourceAmounts1.WoodAmount - resourceAmounts2.WoodAmount,
                resourceAmounts1.StoneAmount - resourceAmounts2.StoneAmount,
                resourceAmounts1.GoldAmount - resourceAmounts2.GoldAmount);

            return resourceValues3;

        }

        // Does not include _amountLimit
        public static bool operator ==(ResourceAmounts resourceAmounts1,
            ResourceAmounts resourceAmounts2)
        {
            return (resourceAmounts1.ColonistAmount == resourceAmounts2.ColonistAmount &&
                    resourceAmounts1.FoodAmount == resourceAmounts2.FoodAmount &&
                    resourceAmounts1.WaterAmount == resourceAmounts2.WaterAmount &&
                    resourceAmounts1.IronAmount == resourceAmounts2.IronAmount &&
                    resourceAmounts1.WoodAmount == resourceAmounts2.WoodAmount &&
                    resourceAmounts1.StoneAmount == resourceAmounts2.StoneAmount &&
                    resourceAmounts1.GoldAmount == resourceAmounts2.GoldAmount);
        }

        // Does not include _amountLimit
        public static bool operator !=(ResourceAmounts resourceAmounts1,
            ResourceAmounts resourceAmounts2)
        {
            return !(resourceAmounts1 == resourceAmounts2);
        }

        // Does not include _amountLimit
        public override bool Equals(object objectToCompare)
        {
            return (objectToCompare is ResourceAmounts &&
                    this == (ResourceAmounts)objectToCompare);
        }

        // May be erroneous!!
        public override int GetHashCode()
        {
            return (AmountLimit.GetHashCode() ^
                    ColonistAmount.GetHashCode() ^
                    FoodAmount.GetHashCode() ^
                    WaterAmount.GetHashCode() ^
                    IronAmount.GetHashCode() ^
                    WoodAmount.GetHashCode() ^
                    StoneAmount.GetHashCode() ^
                    GoldAmount.GetHashCode());

        }
        public override string ToString()
        {
            return "ResourceAmount:" + "\n" +
                   "Limit: " + AmountLimit + "\n" +
                   "Gold: " + GoldAmount + "\n" +
                   "Colonist: " + ColonistAmount + "\n" +
                   "Water: " + WaterAmount + "\n" +
                   "Food: " + FoodAmount + "\n" +
                   "Wood: " + WoodAmount + "\n" +
                   "Stone: " + StoneAmount + "\n" +
                   "Iron: " + IronAmount;
        }
        #endregion
        /// <summary> Returns total amount of items(except gold)
        /// </summary>
        public int GetTotalAmount()
        {
            return ColonistAmount + WaterAmount + FoodAmount + IronAmount + WoodAmount + StoneAmount;
        }

        /// <summary> Returns the unoccupied item space</summary>
        public int GetSpaceLeft()
        {
            return AmountLimit - GetTotalAmount();
        }

        public void SetAmountsToPlayerDefault()
        {
            AmountLimit = GameManager.Instance.gameData.ResourceCapacity;
            ColonistAmount = GameManager.Instance.gameData.DefaultResources[(int) ResourceType.Colonist];
            WaterAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Water];
            FoodAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Food];
            WoodAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Wood];
            StoneAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Stone];
            IronAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Iron];
            GoldAmount = GameManager.Instance.gameData.DefaultResources[(int)ResourceType.Gold];
        }

        /// <summary> Adds given amount of given resourceType
        /// <para>Returns false if there isn't enough space for given amount(except gold) or there is a resource mismatch, true otherwise</para>
        /// </summary>
        public bool AddResource(ResourceType resourceType, int amount)
        {
            if (resourceType!=ResourceType.Gold && GetSpaceLeft() < amount)
            {
                return false;
            }

            switch (resourceType)
            {
                case ResourceType.Colonist:
                    ColonistAmount += amount;
                    break;
                case ResourceType.Water:
                    WaterAmount += amount;
                    break;
                case ResourceType.Food:
                    FoodAmount += amount;
                    break;
                case ResourceType.Wood:
                    WoodAmount += amount;
                    break;
                case ResourceType.Stone:
                    StoneAmount += amount;
                    break;
                case ResourceType.Iron:
                    IronAmount += amount;
                    break;
                case ResourceType.Gold:
                    GoldAmount += amount;
                    break;
                default:
                    return false;
            }

            return true;
        }
        
        /// <summary> Adds multiple types of resources once with given amounts 
        /// <para>Returns false if there isn't enough space for any type or there is a resource mismatch, true otherwise</para>
        /// </summary>
        public bool AddResource(Dictionary<ResourceType, int> resourceAmounts, out string result)
        {
            int totalIncomingAmount = 0;
            foreach (var resourceAmountPair in resourceAmounts)
            {
                if (resourceAmountPair.Key!=ResourceType.Gold)
                    totalIncomingAmount += resourceAmountPair.Value;
            }
            if (GetSpaceLeft() < totalIncomingAmount)
            {
                result = "You don't have enough storage space to do this action";
                return false;
            }

            foreach (var resourceAmountPair in resourceAmounts)
            {
                switch (resourceAmountPair.Key)
                {
                    case ResourceType.Colonist:
                        ColonistAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Water:
                        WaterAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Food:
                        FoodAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Wood:
                        WoodAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Stone:
                        StoneAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Iron:
                        IronAmount += resourceAmountPair.Value;
                        break;
                    case ResourceType.Gold:
                        GoldAmount += resourceAmountPair.Value;
                        break;
                }
            }
            
            result = "Successful";
            return true;
        }

        /// <summary> Removes given amount of given resourceType
        /// <para> Returns false if there isn't enough resource for given amount or there is a resource mismatch, true otherwise</para>
        /// </summary>
        public bool RemoveResource(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Colonist:
                    if (ColonistAmount < amount)
                        return false;
                    ColonistAmount -= amount;
                    break;
                case ResourceType.Water:
                    if (WaterAmount < amount)
                        return false;
                    WaterAmount -= amount;
                    break;
                case ResourceType.Food:
                    if (FoodAmount < amount)
                        return false;
                    FoodAmount -= amount;
                    break;
                case ResourceType.Wood:
                    if (WoodAmount < amount)
                        return false;
                    WoodAmount -= amount;
                    break;
                case ResourceType.Stone:
                    if (StoneAmount < amount)
                        return false;
                    StoneAmount -= amount;
                    break;
                case ResourceType.Iron:
                    if (IronAmount < amount)
                        return false;
                    IronAmount -= amount;
                    break;
                case ResourceType.Gold:
                    if (GoldAmount < amount)
                        return false;
                    GoldAmount -= amount;
                    break;
                default:
                    return false;
            }

            return true;
        }

        /// <summary> Removes multiple types of resources with given amounts
        /// <para> Returns false if there isn't enough resource for any resource type or there is a resource mismatch, true otherwise</para>
        /// </summary>
        public bool RemoveResource(Dictionary<ResourceType, int> resourceAmounts, out string result)
        {
            result = "You don't have enough resource to do this action";
            foreach (var resourceAmountPair in resourceAmounts)
            {
                switch (resourceAmountPair.Key)
                {
                    case ResourceType.Colonist:
                        if (ColonistAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Water:
                        if (WaterAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Food:
                        if (FoodAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Wood:
                        if (WoodAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Stone:
                        if (StoneAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Iron:
                        if (IronAmount < resourceAmountPair.Value)
                            return false;
                        break;
                    case ResourceType.Gold:
                        if (GoldAmount < resourceAmountPair.Value)
                            return false;
                        break;
                }
            }
            foreach (var resourceAmountPair in resourceAmounts)
            {
                switch (resourceAmountPair.Key)
                {
                    case ResourceType.Colonist:
                        ColonistAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Water:
                        WaterAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Food:
                        FoodAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Wood:
                        WoodAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Stone:
                        StoneAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Iron:
                        IronAmount -= resourceAmountPair.Value;
                        break;
                    case ResourceType.Gold:
                        GoldAmount -= resourceAmountPair.Value;
                        break;
                }
            }
            result = "Successful";
            return true;
        }
        
        /// <summary> Adds given amount of given resourceType and removes its gold worth
        /// <para> Returns true if operation is successful</para>
        /// </summary>
        public bool SellResource(ResourceType resourceType, int amount, out string message)
        {
            switch (resourceType)
            {
                case ResourceType.Water when WaterAmount < amount:
                    message = "You don't have enough water";
                    return false;
                case ResourceType.Water:
                    WaterAmount -= amount;
                    GoldAmount += amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Water];
                    break;
                case ResourceType.Food when FoodAmount < amount:
                    message = "You don't have enough food";
                    return false;
                case ResourceType.Food:
                    FoodAmount -= amount;
                    GoldAmount += amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Food];
                    break;
                case ResourceType.Wood when WoodAmount < amount:
                    message = "You don't have enough wood";
                    return false;
                case ResourceType.Wood:
                    WoodAmount -= amount;
                    GoldAmount += amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Wood];
                    break;
                case ResourceType.Stone when StoneAmount < amount:
                    message = "You don't have enough stone";
                    return false;
                case ResourceType.Stone:
                    StoneAmount -= amount;
                    GoldAmount += amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Stone];
                    break;
                case ResourceType.Iron when IronAmount < amount:
                    message = "You don't have enough iron";
                    return false;
                case ResourceType.Iron:
                    IronAmount -= amount;
                    GoldAmount += amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Iron];
                    break;
                default:
                    message = "You dont have enough resources";
                    return false;
            }
            message = "You sold " + resourceType.ToString();
            return true;
        }

        /// <summary> Removes given amount of given resourceType and adds its gold worth
        /// <para> Returns true if operation is successful</para>
        /// </summary>
        public bool BuyResource(ResourceType resourceType, int amount, out string message)
        {
           
            int goldCost;
            switch (resourceType)
            {
                case ResourceType.Colonist :
                    if (FoodAmount < amount * 1 || IronAmount < amount * 1)
                    {
                        message = "You dont have enough resources";
                        return false; 
                    }
                    ColonistAmount += amount;
                    // TODO: Resource cost of colonist should be updated dynamically
                    FoodAmount -= amount * 1;
                    IronAmount -= amount * 1;
                    break;
                case ResourceType.Water:
                    goldCost =
                        amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int) ResourceType.Water];
                    if (GoldAmount < goldCost)
                    {
                        message = "You dont have enough resources";
                        return false;
                    }
                    WaterAmount += amount;
                    GoldAmount -= goldCost;
                    break;
                case ResourceType.Food:
                    goldCost = amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Food]; 
                    if (GoldAmount < goldCost)
                    {
                        message = "You dont have enough resources";
                        return false;
                    }
                    FoodAmount += amount;
                    GoldAmount -= goldCost;
                    break;
                case ResourceType.Wood:
                    goldCost = amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Wood];
                    if (GoldAmount < goldCost)
                    {
                        message = "You dont have enough resources";
                        return false;
                    }
                    WoodAmount += amount;
                    GoldAmount -= goldCost;
                    break;
                case ResourceType.Stone:
                    goldCost = amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Stone];
                    if (GoldAmount < goldCost)
                    {
                        message = "You dont have enough resources";
                        return false;
                    }
                    StoneAmount += amount;
                    GoldAmount -= goldCost;
                    break;
                case ResourceType.Iron:
                    goldCost = amount * GameManager.Instance.gameData.DefaultResourceGoldCosts[(int)ResourceType.Iron];
                    if (GoldAmount < goldCost)
                    {
                        message = "You dont have enough resources";
                        return false;
                    }
                    IronAmount += amount;
                    GoldAmount -= goldCost;
                    break;
                default:
                    message = "Resource type not recognized";
                    return false;
            }
            message = "You bought " + resourceType.ToString();;
            return true;
        }
    }
}
