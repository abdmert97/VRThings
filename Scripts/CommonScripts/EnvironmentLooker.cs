using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonScripts
{
    public class EnvironmentLooker : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler
    { 


        private Vector3 _firstPos;
        private Vector3 _currentPos;
        private Camera _mainCamera;
        public Camera camera;

        public float speed = 3.5f;
        private float X;
        private float Y;
        private bool _enabled = false;
        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if(Input.GetMouseButton(1)&& _enabled)
            {
                ChangeCameraPosition();
            }
            if (Input.GetMouseButtonUp(1))
            {
                _enabled = false;
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Input.GetMouseButton(1))
            {
                _enabled = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _enabled = true;
            }
        }

        private void ChangeCameraPosition()
        {
            camera.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = camera.transform.rotation.eulerAngles.x;
            Y = camera.transform.rotation.eulerAngles.y;
            camera.transform.rotation = Quaternion.Euler(X, Y, 0);

        }

  
    }
}
