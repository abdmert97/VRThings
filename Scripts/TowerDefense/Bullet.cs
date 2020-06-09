using UnityEngine;

namespace TowerDefense
{
    public class Bullet : MonoBehaviour
    {
        [Header("Setup")]
        public GameObject trailEffect;
        [Header("Stats")]
        public float speed = 70f;
        public float damage = 100f;
    
        private Transform target;
        private GameObject effectInst;

        private void Start()
        {
            if(trailEffect)
            {
                effectInst = (GameObject)Instantiate(trailEffect, transform.position, transform.rotation);
            }
        }

        public void Seek (Transform _target) 
        {
            target = _target;
        }

        // Update is called once per frame
        void Update() 
        {
            if (!target) 
            {
                if (trailEffect)
                {
                    Destroy(effectInst);
                }
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distThisFrame = speed * Time.deltaTime;

            if(dir.magnitude <= distThisFrame)
            {
                HitTarget();
                return;
            }
            if (trailEffect)
            {
                effectInst.transform.Translate(dir.normalized * distThisFrame, Space.World);
                effectInst.transform.rotation = Quaternion.Euler(dir.normalized * 100);
            }
            transform.Translate(dir.normalized * distThisFrame, Space.World);
        }

        private void HitTarget() 
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            if (trailEffect)
            {
                Destroy(effectInst);
            }
            Destroy(gameObject);
        }

        public void setDamage(float damage)
        {
            this.damage = damage;
        }
    }
}
