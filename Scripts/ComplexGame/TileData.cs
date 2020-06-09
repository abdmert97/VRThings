using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    [CreateAssetMenu(fileName = "TileData", menuName = "TileDatas", order = 65)]
    public class TileData : ScriptableObject
    {
        [SerializeField] List<GameObject> desertPrefab;
        [SerializeField] List<GameObject> lakePrefab;
        [SerializeField] List<GameObject> fieldPrefab;
        [SerializeField] List<GameObject> forestPrefab;
        [SerializeField] List<GameObject> mountainPrefab;
        [SerializeField] List<GameObject> hillPrefab;

        [SerializeField] GameObject housePrefab;
    
        public List<GameObject> DesertPrefab
        {
            get
            {
                return desertPrefab;
            }
        }

        public List<GameObject> LakePrefab
        {
            get
            {
                return lakePrefab;
            }
        }

        public List<GameObject> FieldPrefab
        {
            get
            {
                return fieldPrefab;
            }
        }

        public List<GameObject> ForestPrefab
        {
            get
            {
                return forestPrefab;
            }
        }

        public List<GameObject> MountainPrefab
        {
            get
            {
                return mountainPrefab;
            }
        }

        public List<GameObject> HillPrefab
        {
            get
            {
                return hillPrefab;
            }
        }

        public GameObject HousePrefab
        {
            get
            {
                return housePrefab;
            }
        }
        
    

        
    }
}
