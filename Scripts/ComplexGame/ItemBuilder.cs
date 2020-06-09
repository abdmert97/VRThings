using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ComplexGame
{
    
    public class ItemBuilder : MonoBehaviour
    {
        public GameObject explorerPrefab;



        public GameObject CreateExplorer(Transform parent, Vector3 position,Player player)
        {
            GameObject explorer = Instantiate(explorerPrefab, parent);
            Interact interact = explorer.AddComponent<Interact>();
            interact.InteractableWith.Add(parent.parent.gameObject);
            interact.interactType = Interact.InteractType.Click;
            explorer.GetComponentInChildren<Renderer>().sharedMaterial =
                GameManager.Instance.hexMap.playerMaterials[player.PlayerNumber];    
            explorer.name = "explorer";
            explorer.tag = "Explorer";
            explorer.transform.localPosition = position;
            explorer.transform.LookAt(Vector3.zero);
            return explorer;



        }
    }

}
