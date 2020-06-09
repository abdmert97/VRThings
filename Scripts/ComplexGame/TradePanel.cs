using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ComplexGame
{
    public class TradePanel : MonoBehaviour
    {
        public static event Action<ResourceType> BuyEvent;
        public static event Action<ResourceType> SellEvent;

        public void BuyFood()
        {
            SFXManager.Instance.Buy_Sell();
            BuyEvent?.Invoke(ResourceType.Food);
        }
        public void SellFood()
        {
            SFXManager.Instance.Buy_Sell();
            SellEvent?.Invoke(ResourceType.Food);
        }
        
        public void BuyWater()
        {
            SFXManager.Instance.Buy_Sell();
            BuyEvent?.Invoke(ResourceType.Water);
        }
        public void SellWater()
        {
            SFXManager.Instance.Buy_Sell();
            SellEvent?.Invoke(ResourceType.Water);
        }
        
        public void BuyIron()
        {
            SFXManager.Instance.Buy_Sell();
            BuyEvent?.Invoke(ResourceType.Iron);
        }
        public void SellIron()
        {
            SFXManager.Instance.Buy_Sell();
            SellEvent?.Invoke(ResourceType.Iron);
        }
        
        public void BuyWood()
        {
            SFXManager.Instance.Buy_Sell();
            BuyEvent?.Invoke(ResourceType.Wood);
        }
        public void SellWood()
        {
            SFXManager.Instance.Buy_Sell();
            SellEvent?.Invoke(ResourceType.Wood);
        }
        
        public void BuyStone()
        {
            SFXManager.Instance.Buy_Sell();
            BuyEvent?.Invoke(ResourceType.Stone);
        }
        public void SellStone()
        {
            SFXManager.Instance.Buy_Sell();
            SellEvent?.Invoke(ResourceType.Stone);
        }

        private void OnDisable()
        {
            BuyEvent = null;
            SellEvent = null;
        }
    }
}
