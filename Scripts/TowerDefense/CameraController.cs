using UnityEngine;

namespace TowerDefense
{
    public class CameraController : MonoBehaviour 
    {

        private bool panActive = true;

        [Header("Horizontal Camera Controls")]
        public float panSpeed = 30f;
        public float panBorderThickness = 10f;
    
        [Header("Vertical Camera Controls")]
        public float scrollSpeed = 5f;
        public float minY = 10f;
        public float maxY = 90f;

        [Header("Key Bindings")]
        public string cameraForwardKey = "w";
        public string cameraBackwardKey = "s";
        public string cameraLeftKey = "a";
        public string cameraRightKey = "d";

        // Update is called once per frame
        void Update() 
        {
            if(GameManager.GameOver)
            {
                return;
            }
            if (Input.GetKey("c")) 
            {
                panActive = !panActive;
            }
            if (!panActive)
                return;

            if(Input.mousePosition.y < 0 || Input.mousePosition.y >= Screen.height)
            {
                return;
            }

            if (Input.mousePosition.x < 0 ||  Input.mousePosition.x >= Screen.width)
            {
                return;
            }

            if (Input.GetKey(cameraForwardKey) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(cameraBackwardKey) || Input.mousePosition.y <= panBorderThickness)
            {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(cameraRightKey) || Input.mousePosition.x >= Screen.width - panBorderThickness) 
            {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(cameraLeftKey) || Input.mousePosition.x <= panBorderThickness) 
            {
                transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }

        
            float scroll = Input.GetAxis("Mouse ScrollWheel");
        
            Vector3 pos = transform.position;
            pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        
            transform.position = pos;

            // Tilt x axis when player zooms the camera
            transform.rotation = Quaternion.Euler(pos.y-10, 0f, 0f);
        }
    }
}
