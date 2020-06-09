using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense
{
    public class Base : MonoBehaviour 
    {
        // Change them to different Materials
        // Set their shared materials not materials
        public Color hoverColor;
        public Color selectColor;
        public Color defaultColor;
        private Renderer renderer;

        private GameObject tower;
        private TowerBlueprint towerBlueprint;
        public Vector3 towerPosOffset;


        BuildManager buildManager;
        private void Start() 
        {
            renderer = GetComponent<Renderer>();
            defaultColor = renderer.material.color;
            buildManager = BuildManager.instance;
        }

        private void OnMouseEnter()
        {
            if(!IsPressable())
            {
                return;
            }
            if (renderer.material.color == defaultColor)
            {
                renderer.material.color = hoverColor;
            }
        }

        private void OnMouseExit() 
        {
            if(renderer.material.color == hoverColor)
            {
                renderer.material.color = defaultColor;
            }
        }
    
        private void OnMouseDown() 
        {
            if (!IsPressable())
            {
                return;
            }
            SFXManager.Instance.Click();
            buildManager.SelectBase(this);
        }

        private bool IsPressable()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsEmpty()
        {
            if (tower)
            {
                return false;
            }
            return true;
        }

        public void BuildTower(TowerBlueprint towerToBuild)
        {
            towerBlueprint = towerToBuild;
            tower = (GameObject)Instantiate(towerBlueprint.level1Prefab, transform.position + towerPosOffset, transform.rotation);
            renderer.material.color = defaultColor;
        }

        public void UpgradeTower()
        {
            Destroy(tower);
            tower = (GameObject)Instantiate(towerBlueprint.level2Prefab, transform.position + towerPosOffset, transform.rotation);
        }
        public void DestroyTower()
        {
            towerBlueprint = null;
            Destroy(tower);
            tower = null;
        }
    }
}
