using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{ 
    public class GameManager : MonoBehaviour
    {
        public static bool GameOver;

        public GameObject gameOverUI;
        public Text enemiesKilled;
        public Text wavesSurvived;
        public List<GameObject> enemyList;
        public static GameManager instance;
        public float groundLevel;
        private void Awake()
        {
            if (instance)
            {
                Debug.LogError("Only one GameManager can exist");
            }
            instance = this;
        }

        private void Start()
        {
            enemyList = new List<GameObject>();
            GameOver = false;
        }

        public static void EndGame()
        {
            if(GameOver)
            {
                return;
            }

            SFXManager.Instance.Game_Finished();

            GameOver = true;
            GameManager.instance.wavesSurvived.text = PlayerStatus.wavesSurvived.ToString();
            GameManager.instance.enemiesKilled.text = PlayerStatus.enemiesKilled.ToString();
            GameManager.instance.gameOverUI.SetActive(true);
        }

        public void Retry()
        {
            PlayerStatus.ResetStatus();
            SceneManager.LoadScene(0);
        }
    }
}
