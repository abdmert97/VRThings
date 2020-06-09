using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace ComplexGame
{

    public class VRManager : MonoBehaviour
    {
        
        public static VRManager instance = null;

        public float camera_offset = 150;

        public Transform canvasTransform;
        public Canvas canvas;

        public OVRGazePointer vrGazePointer;
        public OVRInputModule vrInputModule;

        void Awake()
        {
            if(instance)
            {
                Destroy(instance.gameObject);
            }

            instance = this;
        }

        
        void Start()
        {
            Assert.IsNotNull(canvasTransform);
            Assert.IsNotNull(canvas);
            
            Assert.IsNotNull(vrGazePointer);
            Assert.IsNotNull(vrInputModule);

            Camera cam = Camera.main;
            canvas.worldCamera = cam;
            //vrGazePointer.rayTransform = cam.transform;
            //vrInputModule.rayTransform = cam.transform;
        }


        public void EndTurn()
        {
            canvasTransform.RotateAround(canvasTransform.position, canvasTransform.up, 90);
            
            Camera cam = Camera.main;
            canvas.worldCamera = cam;
            vrGazePointer.rayTransform = cam.transform;
            vrInputModule.rayTransform = cam.transform;
        }
        
    }

}
