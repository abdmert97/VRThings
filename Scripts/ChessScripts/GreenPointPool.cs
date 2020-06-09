using System.Collections.Generic;
using UnityEngine;

namespace ChessScripts
{
    public class GreenPointPool : MonoBehaviour
    {
        [SerializeField] GameObject GreenPoint;

        Queue<GameObject> ActiveGreenPoints = new Queue<GameObject>();
        Queue<GameObject> FreeGreenPoints = new Queue<GameObject>();

        private void Start()
        {
            transform.position = GameObject.FindGameObjectWithTag("Board").transform.position;
        }
        public GameObject GetGreenPoint(Vector3 point)
        {
            if(FreeGreenPoints.Count == 0)
            {
                GameObject greenPoint = Instantiate(GreenPoint, transform);
                greenPoint.transform.localPosition = point;
                ActiveGreenPoints.Enqueue(greenPoint);
                return greenPoint;
            }
            else
            {
                GameObject greenPoint = FreeGreenPoints.Dequeue();
            
                greenPoint.transform.localPosition = point;
                greenPoint.SetActive(true);
                ActiveGreenPoints.Enqueue(greenPoint);
                return greenPoint;

            }
        }
        public void FreePool()
        {
            while(ActiveGreenPoints.Count >0)
            {
                GameObject greenPoint = ActiveGreenPoints.Dequeue();
                greenPoint.SetActive(false);
                FreeGreenPoints.Enqueue(greenPoint);
            }
        }

    }
}
