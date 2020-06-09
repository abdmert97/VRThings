using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class PlayerStatus : MonoBehaviour
    {
        [Header("Status")]
        public static int lives;
        public int startLives = 20;

        public static float Money;
        public float startMoney = 100;

        public static int enemiesKilled = 0;
        public static int wavesSurvived = 0;

        [Header("Settings")]
        public Text moneyUI;
        public Text livesUI;


        public static PlayerStatus instance;

        private void Awake()
        {
            if (instance)
            {
                Debug.LogError("Only one PlayerStatus can exist");
            }
            instance = this;
        }
        void Start()
        {
            lives = startLives;
            UpdateLivesUI();
            Money = startMoney;
            UpdateMoneyUI();
        }

        public static void TakeDamage(int damage)
        {
            lives -= damage;
            PlayerStatus.instance.UpdateLivesUI();
            if (lives <= 0)
            {
                GameManager.EndGame();
            }
        }

        public static void ResetStatus()
        {
            wavesSurvived = 0;
            enemiesKilled = 0;
            Money = PlayerStatus.instance.startMoney;
            lives = PlayerStatus.instance.startLives;
        }

        private void UpdateLivesUI()
        {
            livesUI.text = lives.ToString();
        }

        private void UpdateMoneyUI()
        {
            moneyUI.text = Money.ToString();
        }

        public static void SpendMoney(float amount)
        {
            Money -= amount;
            PlayerStatus.instance.UpdateMoneyUI();
        }

        public static void GainMoney(float amount)
        {
            Money += amount;
            PlayerStatus.instance.UpdateMoneyUI();
        }

    }
}
