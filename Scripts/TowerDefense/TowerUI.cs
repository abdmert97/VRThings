using UnityEngine;

namespace TowerDefense
{
    public class TowerUI : MonoBehaviour
    {
        public GameObject towerUI;
        private Base target;

        public void SetTarget (Base targetToSet)
        {
            target = targetToSet;
            transform.position = target.transform.position;
            ShowUI();
        }
        public void ShowUI()
        {
            towerUI.SetActive(true);
        }

        public void HideUI ()
        {
            towerUI.SetActive(false);
        }
    }
}
