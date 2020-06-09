    using System;
using System.Collections;
    using ComplexGame;
    using TMPro;
    using UnityEditor;
using UnityEngine;
    
    using UnityEngine.SceneManagement;
using UnityEngine.UI;
    using LightType = UnityEngine.LightType;

    namespace UIManager
{
    public class UIManager : MonoBehaviour
    {
        // Game Instance Singleton
        public enum SCENES
        {
            STARTSCENE,
            CHESS,
            TOWERDEFENSE,
            ONITAMA,
            CHECKERS
        };

        public static event Action OnGameStarted;
        public static event Action OnGameEnded;
        public static int screenWidth = 1920;
        public static int screenHeight = 1080;
       
        [SerializeField] private GameObject panel;
        [SerializeField] private Slider slider;
        [SerializeField] private Text sliderPercent;
        [SerializeField] private Image _watch;
        public TMP_Dropdown resolutionDrop;
        public Slider volumeSlider;
        public Slider lightSlider;
        public Toggle musicActive;
        public GameObject endGamePanel;
        public TextMeshProUGUI endGameText;
        public static UIManager Instance { get; private set; } = null;
    
        private void Awake()
        {
            if (Instance)
            {
                GameObject game = Instance.gameObject;
                Instance = this;
                DontDestroyOnLoad(game);
                return;
            }
            
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Instance = this;
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SetResolution;
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += Disconnect;
            volumeSlider.onValueChanged.AddListener (delegate { SetVolume();});
        }

        private static void Disconnect(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            if (to.name.Equals("StartScene"))
            {
                MultiplayerSetup.Instance.DisconnectFromServer();
            }
        }
        private static void SetResolution(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            Screen.SetResolution(screenWidth, screenHeight, true,60);
            CanvasScaler[] canvases = FindObjectsOfType<CanvasScaler>();
            foreach (var canvas in canvases)
            {
                canvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                canvas.referenceResolution = new Vector2(screenWidth,screenHeight);
                switch ( screenWidth)
                {
                    case 1920:
                        canvas.scaleFactor = 1;
                        break;
                    case 1366:
                        canvas.scaleFactor = 0.7f;
                        break;
                    case 1280:
                        canvas.scaleFactor = 0.63f;
                        break;
                    case 1024:
                        canvas.scaleFactor = 0.51f;
                        break;
                    case 800:
                        canvas.scaleFactor = 0.41f;
                        break;
                }
        
            }
        }

        public void ChangeResolution()
        {
            switch (resolutionDrop.value)
            {
                case 0:
                    screenWidth = 1920;
                    screenHeight = 1080;
                    break;
                case 1:
                    screenWidth = 1366;
                    screenHeight = 768;
                    break;
                case 2:
                    screenWidth = 1280;
                    screenHeight = 800;
                    break;
                case 3:
                    screenWidth = 1024;
                    screenHeight = 768;
                    break;
                case 4:
                    screenWidth = 800;
                    screenHeight = 600;
                    break;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void Start()
        {
            panel.SetActive(false);
            Screen.fullScreen = !Screen.fullScreen;
        }

        
        public void OpenPanel(GameObject panel)
        {
            panel.SetActive(true);
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void LoadScene(int index)
        {
            Load(index);
        }

        public void SetMusic()
        {
            if(musicActive.isOn)
                AudioManager.Instance.PlayMusic();
            else
            {
                AudioManager.Instance.StopMusic();
            }
        }
        public void SetVolume()
        {
            AudioManager.Instance.SetVolume(volumeSlider.value/100f);
        }
        public void SetLight()
        {
            Light[] lights =  FindObjectsOfType<Light>();
            foreach (var light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    light.intensity = lightSlider.value;
                    
                }
                
            }
        
        }
        public void LoadGame(SCENES scene)
        {
            Load((int) scene);
            OnGameStarted?.Invoke();
        }

        public void QuitGame()
        {
            SceneManager.LoadScene((int) SCENES.STARTSCENE, LoadSceneMode.Single);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }


        public void Load(int index)
        {
            StartCoroutine(LoadSceneAsync(index));
        }

        IEnumerator LoadSceneAsync(int index)
        {
            panel.SetActive(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(index);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                _watch.rectTransform.Rotate(Vector3.forward * 2);
                yield return null;
            }
          for (int i = 0; i < 180; i++)
            {
                _watch.rectTransform.Rotate(Vector3.forward * 2);

                yield return null;
            }
            panel.SetActive(false);
        }

        public void OpenEndGamePanel(string text = "Game Ended")
        {
            endGamePanel.SetActive(true);
            endGameText.text = text;
        }
    }
}