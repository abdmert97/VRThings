using System;
using UnityEngine;

namespace ComplexGame
{
    public class Interactable : MonoBehaviour
    {
        public static Action MouseDown;
        public static Action MouseStay;
        public static Action MouseUp;

        public Camera currentCamera;
        public int layerMask;
        private readonly string _emptyString = "";
        private readonly string _defaultLayer = "Default";
        private int _notInteractableLayer;
        private int _interactableLayer;

        public static Action<GameObject,Interact.InteractType>[] InteractWithObject;
        public static Action<GameObject,Interact.InteractType>[] EndInteractWithObject;

        private void Awake()
        {
            _notInteractableLayer = LayerMask.GetMask("NotInteractable");
            _interactableLayer = ~_notInteractableLayer;
            if (currentCamera == null)
                currentCamera = Camera.main;
   
            InteractWithObject = new Action<GameObject,Interact.InteractType>[GameManager.Instance.gameData.PlayerCount];
            EndInteractWithObject = new Action<GameObject,Interact.InteractType>[GameManager.Instance.gameData.PlayerCount];
        }

        /// <summary> It send raycast from current camera according to mouse position</summary>
        /// <param name="layerName">Indicates raycast layer if empty layer equals to default.(optional)</param>
        /// 
        /// <param name="rayFunction">If ray hits the object this function is called.(optional)</param>
        public RaycastHit GetRaycast(string layerName = "",Action<RaycastHit> rayFunction = null)
        {
            if (!layerName.Equals(_emptyString))
                layerMask = LayerMask.GetMask(layerName);
            else // Default layerMask
                layerMask = _interactableLayer;
        
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Does the ray intersect any objects 
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                rayFunction?.Invoke(hit);
                return hit;
            }

            return hit;
        }
        /// <summary> It send raycast from current camera according to mouse position</summary>
        /// <param name="layerName">Indicates raycast layer if empty layer equals to default.(optional)</param>
        /// 
        /// <param name="rayFunction">If ray hits the object this function is called.(optional)</param>
        /// 
        /// <param name="tag">If ray hit the object with tag</param>
        public void GetRaycast(string tag,string layerName = "", Action<RaycastHit> rayFunction = null)
        {
            if (!layerName.Equals(_emptyString))
                layerMask = LayerMask.GetMask(layerName);
            else // Default layerMask
                layerMask = _interactableLayer;

            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Does the ray intersect any objects 
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if(hit.collider.CompareTag(tag))
                    rayFunction?.Invoke(hit);
            }

        }
        /// <summary> It send raycast from current camera according to mouse position</summary>
        /// <param name="layerName">Indicates raycast layer if empty layer equals to default.(optional)</param>
        /// 
        /// <param name="rayFunction">If ray hits the object this function is called.(optional)</param>
        /// 
        /// <param name="name">If ray hit the object with name</param>
        public bool GetRaycastByName(string name, string layerName = "", Action<RaycastHit> rayFunction = null)
        {
            if (!layerName.Equals(_emptyString))
                layerMask = LayerMask.GetMask(layerName);
            else // Default layerMask
                layerMask = _interactableLayer;

            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Does the ray intersect any objects 
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.collider.name.Equals(name))
                {
                    rayFunction?.Invoke(hit);
                    return true;
                }
            }
            return false;
        }
    
    }
}
