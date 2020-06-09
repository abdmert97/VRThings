using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ComplexGame
{
    public class InputHandler : MonoBehaviour
    {
        private TurnManager _turnManager;
 
        private Interactable _interactable;
        private void Start()
        {
            _turnManager = FindObjectOfType<TurnManager>();
            _interactable = FindObjectOfType<Interactable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {   
                InputInfo inputInfo = new InputInfo();
                RaycastHit hit;
                FillInfo(ref inputInfo);
                hit =_interactable.GetRaycast();
                if(hit.collider != null)
                    inputInfo.hit = hit.collider.gameObject;
                GameManager.Instance.playerList[_turnManager.TurnFor].InvokeHandleInput(inputInfo);
            }
        }

        private void FillInfo(ref InputInfo inputInfo)
        {
            inputInfo.mousePosition = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                inputInfo.leftClick = true;
            }
            if (Input.GetMouseButtonDown(1))
            {
                inputInfo.rightClick = true;
            }
        }
    }

    public struct InputInfo
    {
        public Vector2 mousePosition;
        public GameObject hit;
        public bool leftClick;
        public bool rightClick;
        public bool isKeyDown;
   
    }
}