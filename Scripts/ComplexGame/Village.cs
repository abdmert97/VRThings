using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    public enum VillageType
    {
        Hill,
        Fields,
        Mountain,
        Forest,
        Farm
    }
    public class Village : MonoBehaviour
    {
        public VillageType villageType;
        private Player _owner;
    }
}