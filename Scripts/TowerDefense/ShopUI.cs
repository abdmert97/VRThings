using UnityEngine;

namespace TowerDefense
{
    public class ShopUI : MonoBehaviour
    {
        public GameObject shopUI;
        private Base target;

        public void SetTarget(Base targetToSet)
        {
            target = targetToSet;
            transform.position = target.transform.position;
            ShowUI();
        }
        public void ShowUI()
        {
            shopUI.SetActive(true);
        }

        public void HideUI()
        {
            shopUI.SetActive(false);
        }
    }
}
