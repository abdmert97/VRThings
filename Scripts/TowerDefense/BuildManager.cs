using UnityEngine;

namespace TowerDefense
{
    public class BuildManager : MonoBehaviour 
    {

        private TowerBlueprint towerToBuild;
        private Base selectedBase;
        public static BuildManager instance;
        public GameObject cannonTowerPrefab;
        public GameObject mageTowerPrefab;

        public TowerUI towerUI;
        public ShopUI shopUI;

        private void Awake() 
        {
            if (instance)
            {
                Debug.LogError("Only one BuildManager can exist");
            }
            instance = this;
        }



        public void SelectBase(Base baseToSelect)
        {
            if(selectedBase == baseToSelect)
            {
                UnselectBase();
                return;
            }
            else if(selectedBase != null)
            {
                UnselectBase();
            }
            selectedBase = baseToSelect;
            selectedBase.GetComponent<Renderer>().material.color = selectedBase.selectColor;
            if (baseToSelect.IsEmpty())
            {
                shopUI.SetTarget(baseToSelect);
            }
            else
            {
                towerUI.SetTarget(baseToSelect);
            }
        }

        public void UnselectBase()
        {
            selectedBase.GetComponent<Renderer>().material.color = selectedBase.defaultColor;
            selectedBase = null;
            towerUI.HideUI();
            shopUI.HideUI();
        }



        public void BuildTower(TowerBlueprint tower)
        {
            
            SFXManager.Instance.Tower_Build();

            towerToBuild = tower;
            selectedBase.BuildTower(towerToBuild);
            UnselectBase();
        }

        public void UpgradeTower()
        {
            SFXManager.Instance.Tower_Upgraded();

            selectedBase.UpgradeTower();
            UnselectBase();
        }

        public void DestroyTower()
        {
            SFXManager.Instance.Tower_Destroyed();

            selectedBase.DestroyTower();
            UnselectBase();
        }
    }
}
