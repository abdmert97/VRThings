using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommonScripts
{
    

    public class CamMotion : MonoBehaviour
    {
        public float speedH = 2.0f;
        public float speedY = 2.0f;

        private float yaw = 0;
        private float pitch = 45;

        private bool scene1 = false;

        // Start is called before the first frame update
        void Start()
        {
            if(scene1)
            {
                transform.position = new Vector3(2.04f, 9.44f, -5.19f);
                transform.eulerAngles = new Vector3(pitch,yaw,0f);
            }
            else
            {
                transform.position = new Vector3(2.05f, 9.83f,-5.34f);
                transform.eulerAngles = new Vector3(pitch,yaw,0f);
            }
        }

        // Update is called once per frame
        void Update()
        {

            if(Input.GetMouseButton(0)&& Input.GetKey(KeyCode.LeftControl))
            {
                yaw += speedH * -Input.GetAxis("Mouse X");
                pitch += speedY * -Input.GetAxis("Mouse Y");

                transform.eulerAngles = new Vector3(pitch,yaw,0f);
            }
            else if (Input.GetKey(KeyCode.LeftControl))

            {
                transform.Rotate(Vector3.up*20*Time.deltaTime,Space.World);
            }


        }
    }
}