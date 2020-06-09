using System.Collections.Generic;
using UnityEngine;

namespace CommonScripts
{
    [CreateAssetMenu(fileName = "EnviromentData", menuName = "EnviromentDatas", order = 61)]

    public class EnviromentData : ScriptableObject
    {
        public List<GameObject> enviroments;
    }
}
