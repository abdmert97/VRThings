using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComplexGame
{
    [RequireComponent(typeof(Collider))] 
    public class Interact : MonoBehaviour
    {
        public enum InteractType
        {
            Click,
            Drag,
            Throw,
        }
        
        public List<GameObject> InteractableWith= new List<GameObject>();
        private Vector3 _offset;
        private float _zCoordinate;
        public InteractType interactType;
        private Rigidbody _rigidbody;
        private Vector3 _currPos;
        private Queue<float> _time;
        private Vector3 _lastPos;
        private Vector3 _firstPos;
        private bool _drag = false;
        private Vector3 _positionBeforeDrag;
        private Camera camera;
        public bool isEnabled = false;
        public bool isReleased = false;
        private bool isInInteract = false;
        private void Start()
        {
            camera = Camera.main;
            _time = new Queue<float>();
            _rigidbody = GetComponent<Rigidbody>();
            _positionBeforeDrag = transform.position;

        }
        private void Update()
        {
            if(_drag)
                TimeCounter();
        }
        
        public void OnMouseDown()
        {
            SFXManager.Instance.Click();
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            _positionBeforeDrag = transform.position;
            isReleased = false;
             
            if (!InteractableWith.Contains( GameManager.Instance.GetCurrentPlayer()))
            {
               return;
            }
            
            Interactable.InteractWithObject[GameManager.Instance.GetCurrentPlayerNumber()].Invoke(gameObject,interactType);
            isInInteract = true;
            if (interactType == InteractType.Click)
            {
                return;
            }
            
            camera = GameManager.Instance.interactable.currentCamera;
            var position = gameObject.transform.position;
            _zCoordinate = camera.WorldToScreenPoint(
                position).z;
            
            _offset = position - GetMouseAsWorldPoint();
            _lastPos = GetMouseAsWorldPoint();
            _firstPos = transform.position;
            if(interactType == InteractType.Throw)
            {
                _drag = true;
                _rigidbody.velocity = Vector3.zero;
            }
        }



   
        public void OnMouseUp()
        {

            isReleased = true;
            if (!isInInteract)
            {
                Invoke(nameof(ResetPosition),0.1f);
                return;
            }

            isInInteract = false;
            if (!InteractableWith.Contains( GameManager.Instance.GetCurrentPlayer())|| interactType == InteractType.Click)
            {
                // connot  interact 
                if (InteractType.Click == interactType)
                {
                      Interactable.EndInteractWithObject[GameManager.Instance.GetCurrentPlayerNumber()].Invoke(gameObject,interactType);
                }
                Invoke(nameof(ResetPosition),0.1f);
                return;
            }
            Interactable.EndInteractWithObject[GameManager.Instance.GetCurrentPlayerNumber()].Invoke(gameObject,interactType);
            if(interactType == InteractType.Throw)
            {
                Throw();
                _time.Clear();
                _drag = false;
           
            }

            Invoke(nameof(ResetPosition),0.1f);
        }

        private void ResetPosition()
        {
            if (isEnabled == false)
            {
                transform.position = _positionBeforeDrag;
            }
        }
        private void Throw()
        {
            float totalForce = 0;
            for (int i = 0; i < _time.Count; i++)
            {
                totalForce += _time.Dequeue();
            }
            if(totalForce>1)
            {
           
                _rigidbody.AddForce(totalForce*(transform.position-_firstPos)*90);
                _rigidbody.AddTorque(new Vector3(UnityEngine.Random.Range(0, 30), UnityEngine.Random.Range(0, 30), UnityEngine.Random.Range(0, 30)) );
            }
        }

        void OnMouseDrag()
        {
            if (!InteractableWith.Contains( GameManager.Instance.GetCurrentPlayer()))
            {
                // connot  interact 
                return;
            }
            if (interactType == InteractType.Click)
                return;
            _offset.z = 0;
            var newPosition = GetMouseAsWorldPoint() + _offset;
            newPosition.y = _positionBeforeDrag.y + 1f;
            transform.position = newPosition;
        }

        private void TimeCounter()
        {
            if (_time.Count > 5)
                _time.Dequeue();
            _currPos = GetMouseAsWorldPoint();
            _time.Enqueue((_currPos - _lastPos).magnitude);
            _lastPos = _currPos;
        }

        private Vector3 GetMouseAsWorldPoint()
        {
            // Pixel coordinates of mouse (x,y)
            Vector3 mousePoint = Input.mousePosition;
            // z coordinate of game object on screen
             mousePoint.z = _zCoordinate;
            // Convert it to world points
            return GameManager.Instance.interactable.currentCamera.ScreenToWorldPoint(mousePoint);

        }
    }
}