using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class WaveSpawner : MonoBehaviour 
    {
        [Header("Enemy")]
        public GameObject enemyPrefab;
        public GameObject enemyLevel2;
        public Vector3 enemyPosOffset;
        public GameObject spawnEffect;

        [Header("Wave")]
        public Text waveNumberUI;
        public Text waveTimerUI;
        public Transform spawnPoint;
        public float waveInterval = 3.4f;
        public float waveThickness = 0.7f;

        private float timer = 1f;   // First spawn in 1 sec
        private float waveNumber = 0;
        private GameManager gameManager;
        private WaitForSeconds waitSeconds;
        private const string wave = "Wave ";
        private GameObject enemies;
        private GameObject effects;
        private void Start()
        {
            enemies = new GameObject("Enemies");
            effects = new GameObject("Effects");
            waitSeconds = new WaitForSeconds(waveThickness);
            gameManager = GameManager.instance;
        }
        void Update()
        {
            if (timer <= 0f) 
            {

                StartCoroutine(SpawnWave());
                PlayerStatus.wavesSurvived++;
                timer = waveInterval;
            }
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer, 0f, Mathf.Infinity);
            waveNumberUI.text = wave + waveNumber;
            waveTimerUI.text = string.Format("{0:00.00}", timer).Replace(',','.');
        }

        IEnumerator SpawnWave() 
        {
            waveNumber++;
            for (int i = 0; i < waveNumber * 2; i++) {
                SpawnEnemy();
                
                yield return waitSeconds;
            }
        }

        void SpawnEnemy()
        {
            // Do not create new effect every time only restart it
            Vector3 position = spawnPoint.position;
            GameObject currentEnemy = waveNumber < 3 ? enemyPrefab : enemyLevel2;
            GameObject enemy = Instantiate(currentEnemy, spawnPoint.position + enemyPosOffset, spawnPoint.rotation).gameObject;
            gameManager.enemyList.Add(enemy);
            enemy.transform.SetParent(enemies.transform);
            GameObject effectInst = (GameObject)Instantiate(spawnEffect, spawnPoint.position, Quaternion.Euler(-90, 0, -90));
            effectInst.transform.SetParent(effects.transform);
            transform.position -= transform.position.y * Vector3.up;
            Destroy(effectInst, 2f);
        }
    }
}