using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour
    {
        public float startSpeed = 5f;
        [HideInInspector]
        public float speed;

        public float startHealth = 100f;
        private float health;
        public float gainMoney = 1f;
 
        [Header("Unity Setup")]
        public GameObject impactEffect;
        public Image healthBar;
        public Canvas healthBarCanvas;
    
        private void Start()
        {
            speed = startSpeed;
            health = startHealth;
        }

        private void Update()
        {
            healthBarCanvas.transform.LookAt(Camera.main.transform);
            healthBarCanvas.transform.Rotate(0, 180, 0);
        }

        public void TakeDamage(float damage)
        {
        
            health -= damage;
            healthBar.fillAmount = health / startHealth;
            // Enemy size scales with its health
            transform.localScale += (-1*Vector3.one) * damage/startHealth;
            if(health <= 0)
            {
                Die();
            }
        }

        public void Slow(float percentage)
        {
            speed = startSpeed*(1f - percentage);
        }

        void Die()
        {
            PlayerStatus.GainMoney(gainMoney);
            PlayerStatus.enemiesKilled++;

            Vector3 effectPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject effectInst = (GameObject)Instantiate(impactEffect, effectPos, Quaternion.Euler(-90f,0f,0f));
            Destroy(effectInst, 2f);
            GameManager.instance.enemyList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
