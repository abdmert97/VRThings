using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    public enum TileType
    {
        Desert, // produces nothing
        Lake,   // produces water costs 3 golds
        Field,  // produces food costs 4 golds
        Forest,  // produces wood costs 5 golds
        Hill,   //produces stone costs 6 golds
        Mountain // produces iron costs 7 golds
    }
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }

        private VillageBlueprint _villageToBuild;
 
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // Get Coordinate of a hex
        
        // Get ID of a region
        public int GetRegionID(GameObject hit)
        {
            return GameManager.Instance.hexMap.GetRegionCoordinate(hit);
        }

        // Build a house to a hex
        public bool BuildHouse(int activePlayerID,Hex coordinate,out string result )
        {
            bool success = GameManager.Instance.hexMap.BuildHouse(activePlayerID,coordinate,out result);
            if (success)
            {
                AnimatedCanvas.Instance.ShowMessage(result);
            }
            else
            {
                AnimatedCanvas.Instance.ShowError(result);
            }
            return success;
        }

        // Build a house to a hex
        public bool BuildHouse(int activePlayerID,Hex coordinate )
        {
            string result ;
            bool success = GameManager.Instance.hexMap.BuildHouse(activePlayerID,coordinate,out result);
            if (success)
            {
                // Increase house count for activePlayer
                GameManager.Instance.playerList[activePlayerID].houseCount++;
                AnimatedCanvas.Instance.ShowMessage(result);
            }
            else
            {
                AnimatedCanvas.Instance.ShowError(result);
            }
            return success;
        }

        // Returns buildings count in a hex
        public int BuildingsCount(Hex coordinate)
        {
            return GameManager.Instance.hexMap.BuildingCount(coordinate);
        }

        // Deactivate all REgions
        // Returns gained golds
        public int DeactivateRegions()
        {
            return GameManager.Instance.hexMap.DeactivateRegions();
        }

        //Actiavte region
        //If it is already actiavted return false
        public bool ActivateRegion(int regionID,int activePlayerID,out Dictionary<ResourceType, int> resource, out string result)
        {
            resource = new Dictionary<ResourceType, int>();
            List<int> r;
            bool action = GameManager.Instance.hexMap.ActivateRegion(regionID,activePlayerID,out r,out result);
            if (action)
            {
                AnimatedCanvas.Instance.ShowMessage(result);
            }
            else
            {
                AnimatedCanvas.Instance.ShowError(result);
            }
            if(action == false)
            {
                return false;
            }
            else
            {
                resource.Add(ResourceType.Water, r[0]);
                resource.Add(ResourceType.Food, r[1]);
                resource.Add(ResourceType.Wood, r[2]);
                resource.Add(ResourceType.Stone, r[3]);
                resource.Add(ResourceType.Iron, r[4]);
                return true;
            }
            
        }

        //Actiavte region
        //If it is already actiavted return false
        public bool ActivateRegion(int regionID,int activePlayerID,out ResourceAmounts resource)
        {
            string result;
            resource = new ResourceAmounts(0,0,0,0,0,0,0);
            List<int> r;
            bool action = GameManager.Instance.hexMap.ActivateRegion(regionID,activePlayerID,out r,out result);
            if (action)
            {
                AnimatedCanvas.Instance.ShowMessage(result);
            }
            else
            {
                AnimatedCanvas.Instance.ShowError(result);
            }
            if(action == false)
            {
                return false;
            }
            else
            {
                resource.WaterAmount = r[0];
                resource.FoodAmount = r[1];
                resource.WoodAmount = r[2];
                resource.StoneAmount = r[3];
                resource.IronAmount = r[4];
                return true;
            }
        }

    }
}