using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;

namespace CommonScripts
{
    public class EnviromentLoader : MonoBehaviour
    {
        public static EnviromentLoader Instance { get; private set; }
        public static EnviromentData enviroments;
        public static int selectedEnviroment;
        public static GameObject created;
     
        private void Awake()
        {
           
            if (Instance)
            {
                GameObject game = Instance.gameObject;
                Instance = this;
                DontDestroyOnLoad(game);
                return;
            }
         
            enviroments = Instantiate(Resources.Load<EnviromentData>("Enviroments"));
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Initialize();
        }
        private static bool initialized = false;
       

        // ensure you call this method from a script in your first loaded scene
        public static void Initialize()
        {
            if (initialized == false)
            {
                initialized = true;
                // adds this to the 'activeSceneChanged' callbacks if not already initialized.
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneWasLoaded;
            }
        }
        private static void OnSceneWasLoaded(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            if(!to.name.Equals("StartScene") &&!to.name.Contains("Room"))
            {
                LoadEnviroment();
            }
        }

        private static void LoadEnviroment()
        {
      
            GameObject[] objectList = FindObjectsOfType<GameObject>();
            ///GameObject parent = new GameObject("GameElements");
            GameObject board = GameObject.FindGameObjectWithTag("Board");
            if (board == null)
            {
                Debug.LogError("Board couldn't found");
            }
       
            /*  for (int i = 0; i < objectList.Length; i++)
        {
            if (objectList[i].GetComponentsInChildren<Renderer>()!=null)
                objectList[i].transform.SetParent(parent.transform);
        }*/
            Bounds maxVolume = CalculateCumulativeBounds(objectList);
            Bounds bound = ComplexGame.BoundCalculator.CalculateCumulativeBounds(board.gameObject);    
         
         
            GameObject environment = Instantiate(enviroments.enviroments[selectedEnviroment]);
            Bounds enviromentVolume = CalculateCumulativeBounds(environment);
            GameObject table = GameObject.FindGameObjectWithTag("Table");
            table.SetActive(true);
            Bounds tableVolume = CalculateCumulativeBounds(table);
            created = environment;

       
            float scaleX = ( maxVolume.size.x/tableVolume.size.x );
            float scaleZ = ( maxVolume.size.z/ tableVolume.size.z);
            float scaleY = Mathf.Min(scaleX, scaleZ);
            Vector3 scale = new Vector3(scaleX , scaleY, scaleZ );
            scale *= 1.4f;        
            environment.transform.localScale = scale;
            environment.transform.position = Vector3.zero;
            environment.name = "Environment";
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            Vector3 center = bound.center;
         
            Vector3 distance =    center - spawnPoint.transform.position;
            distance.y = board.transform.position.y - spawnPoint.transform.position.y-bound.extents.y;
            //distance -= maxVolume.extents.y * Vector3.up;
            environment.transform.position = distance;

   

            GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
         //   GameObject cameraPoint = GameObject.FindGameObjectWithTag("CameraPoint");
         //   Camera.transform.position = cameraPoint.transform.position;
         //   Camera.transform.rotation = cameraPoint.transform.rotation;
        //    Camera.AddComponent<CameraFollower>();
        }
        private static Bounds CalculateCumulativeBounds(GameObject[] parentTransform)
        {
            List<Renderer> rendererList = new List<Renderer>();
            for (int j = 0; j < parentTransform.Length; j++)
            {
                for(int k = 0; k< parentTransform[j].transform.childCount;k++)
                {
                    Transform transform = parentTransform[j].transform.GetChild(k);
                    Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
                    foreach (Renderer render in renderers)
                    {
                        rendererList.Add(render);
                    }
                }
            }
            int rendererCount = rendererList.Count;
            if (rendererList == null || rendererCount == 0)
                return new Bounds();
            Bounds bounds = rendererList[0].bounds;
            for (int i = 1; i < rendererCount; i++)
            {
                bounds.Encapsulate(rendererList[i].bounds);
            }
            return bounds;
        }
        private static Bounds CalculateCumulativeBounds(GameObject parentTransform)
        {
            List<Renderer> rendererList = new List<Renderer>();
        
            for (int k = 0; k < parentTransform.transform.childCount; k++)
            {
                Transform transform = parentTransform.transform.GetChild(k);
                Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renderers)
                {
                    rendererList.Add(render);
                }
            }
      
            int rendererCount = rendererList.Count;
            if (rendererList == null || rendererCount == 0)
                return new Bounds();
            Bounds bounds = rendererList[0].bounds;
            for (int i = 1; i < rendererCount; i++)
            {
                bounds.Encapsulate(rendererList[i].bounds);
            }
            return bounds;
        }
        public  void AddEnviroment(GameObject enviroment)
        {
            return;
            /*
            if (!enviroments.Contains(enviroment))
            {
     
                enviroments.Add(enviroment);
                selectedEnviroment = enviroments.Count-1;
            }
            else
            {
                selectedEnviroment = enviroments.IndexOf(enviroment);
            }
            Debug.Log(enviroments[selectedEnviroment].name);*/
        }
        public  void SetSelectedEnviroment(int selection)
        {
                Debug.Log(selectedEnviroment);
              selectedEnviroment = selection;

        }
        void OnDrawGizmosSelected()
        {
            Debug.Log("Draw Gizmo");
            Gizmos.color = Color.yellow;
            GameObject[] objectList = FindObjectsOfType<GameObject>();
            Debug.Log(objectList.Length);
            Bounds maxVolume = CalculateCumulativeBounds(objectList);
            Gizmos.DrawWireCube(maxVolume.center,maxVolume.size);
            Gizmos.color = Color.red;
            Bounds maxVolume2 = CalculateCumulativeBounds(created);
            Gizmos.DrawWireCube(maxVolume2.center, maxVolume2.size);
        }
    }
}
