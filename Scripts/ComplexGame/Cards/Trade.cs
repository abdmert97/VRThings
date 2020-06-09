using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame.Cards
{
    public enum TradeAction
    {
        Buy,
        Sell
    }
    public class Trade : Card
    {
        private Player _player;
        public override bool Play(Player player, out string message)
        {
            _player = player;
            player.storage.AddResource(ResourceType.Gold, GameManager.Instance.gameData.TradeCardGoldGain);
            
         
            
            InGameUIManager.Instance.OpenTradePanel();
            InGameUIManager.Instance.OpenResourcePanel(player.storage);
            TradePanel.BuyEvent += BuyResource;
            TradePanel.SellEvent += SellResource;
            message = "Trade Started";
            return false;
        }

        private void BuyResource(ResourceType resourceType)
        {
            _player.storage.BuyResource(resourceType, 1, out string message);
            InGameUIManager.Instance.OpenResourcePanel(_player.storage);
            Debug.Log(message);
            AnimatedCanvas.Instance.ShowMessage(message);
        }
        private void SellResource(ResourceType resourceType)
        {
            _player.storage.SellResource(resourceType, 1, out string message);
            InGameUIManager.Instance.OpenResourcePanel(_player.storage);
            Debug.Log(message);
            AnimatedCanvas.Instance.ShowMessage(message);
        }

       
    }
}