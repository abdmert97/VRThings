using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ComplexGame
{
    
    public class InGameUIManager : MonoBehaviour
    {
        
        public static InGameUIManager Instance { get; private set; }
        [SerializeField] InfoPanel infoPanel;
        [SerializeField] CardOptionPanel cardOptionPanel;
        [SerializeField] ResourcePanel resourcePanel;
        [SerializeField] private Sprite explorerSprite;
        [SerializeField] private TradePanel tradePanel;
        public int selectedOption = -1;
        private Animator _infoPanelAnimator;
        private Animator _tradePanelAnimator;
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            _infoPanelAnimator = infoPanel.infoPanel.GetComponent<Animator>();
            _tradePanelAnimator = tradePanel.gameObject.GetComponent<Animator>();
            Instance = this;
        }
        public void OpenInfoPanel(Sprite cardFrontFace, string title, string info)
        {
            infoPanel.cardTitle.text = title;
            infoPanel.cardInfo.text = info;
            if (cardFrontFace == null)
            {
                cardFrontFace = explorerSprite;
            }
            infoPanel.cardImage.sprite = cardFrontFace;
            infoPanel.infoPanel.SetActive(true);
            _infoPanelAnimator.Play("OpenInfoPanel");
            
        }
        public void CloseInfoPanel()
        {
            infoPanel.infoPanel.SetActive(false);
        }

        public void OpenCardOptionPanel(Sprite option1Image, string option1Text, Sprite option2Image, string option2Text)
        {
            cardOptionPanel.option1Image.sprite = option1Image;
            cardOptionPanel.option1Text.text = option1Text;
            cardOptionPanel.option2Image.sprite = option2Image;
            cardOptionPanel.option2Text.text = option2Text;
            cardOptionPanel.cardOptionPanel.SetActive(true);

        }
        public void CloseCardOptionPanel()
        {
            cardOptionPanel.cardOptionPanel.SetActive(false);
        }
        public void OpenResourcePanel(ResourceAmounts playerStorage)
        {
            resourcePanel.resourcePanel.SetActive(true);
            resourcePanel.colonistText.text = playerStorage.ColonistAmount.ToString();
            resourcePanel.waterText.text = playerStorage.WaterAmount.ToString();
            resourcePanel.foodText.text = playerStorage.FoodAmount.ToString();
            resourcePanel.woodText.text = playerStorage.WoodAmount.ToString();
            resourcePanel.stoneText.text = playerStorage.StoneAmount.ToString();
            resourcePanel.ironText.text = playerStorage.IronAmount.ToString();
            resourcePanel.goldText.text = playerStorage.GoldAmount.ToString();
        }

        public void CloseResourcePanel()
        {
            resourcePanel.resourcePanel.SetActive(false);
        }

        public void OpenTradePanel()
        {
            tradePanel.gameObject.SetActive(true);
            _tradePanelAnimator.Play("OpenTradePanel");
        }

        public void CloseTradePanel()
        {
            tradePanel.gameObject.SetActive(false);
        }

    }
    [Serializable]
    public struct InfoPanel
    {
        public GameObject infoPanel;
        public Image cardImage;
        public TextMeshProUGUI cardTitle;
        public TextMeshProUGUI cardInfo;
    }
    [Serializable]
    public struct CardOptionPanel
    {
        public GameObject cardOptionPanel;
        public Image option1Image;
        public TextMeshProUGUI option1Text;
        public Image option2Image;
        public TextMeshProUGUI option2Text;
    }
    [Serializable]
    public struct ResourcePanel
    {
        public GameObject resourcePanel;
       
        public Image colonist;
        public TextMeshProUGUI colonistText;
        
        public Image food;
        public TextMeshProUGUI foodText;
        
        public Image water;
        public TextMeshProUGUI waterText;
        
        public Image iron;
        public TextMeshProUGUI ironText;
        
        public Image wood;
        public TextMeshProUGUI woodText;
        
        public Image stone;
        public TextMeshProUGUI stoneText;
        
        public Image gold;
        public TextMeshProUGUI goldText;
      
    }
}
