using UnityEngine;

namespace TowerDefense
{
    public class Shop : MonoBehaviour
    {
        public TowerBlueprint cannonTower;
        public TowerBlueprint mageTower;
        BuildManager buildManager;
        private void Start()
        {
            buildManager = BuildManager.instance;
        }
        public void PurchaseCannonTower()
        {
            if (MakePurchase(cannonTower.cost))
            {
                buildManager.BuildTower(cannonTower);
            }
        }

        public void PurchaseMageTower()
        {
            if (MakePurchase(mageTower.cost))
            {
                buildManager.BuildTower(mageTower);
            }
        }

        public void UpgradeTower()
        {
            if (MakePurchase(25f))
            {
                buildManager.UpgradeTower();
            }
        }

        public void DestroyTower()
        {
            buildManager.DestroyTower();
        }

        private bool MakePurchase(float cost)
        {
            if(PlayerStatus.Money < cost)
            {
                return false;
            }
            else
            {
                PlayerStatus.SpendMoney(cost);
                return true;
            }
        }
    }
}
