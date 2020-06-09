using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour 
    {
        private Transform target;
        private Enemy targetEnemy;

        [Header("General")]
        public float range = 15f;
        public float rotationSpeed = 10f;

        [Header("CannonTower Settings(default)")]
        public GameObject bulletPrefab;
        public float fireRate = 1f;
        public float cannonDamage = 50f;
        private float fireTimer = 0f;

        [Header("MageTower Settings")]
        public bool useLaser = false;
        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;
        public Light impactLight;
        public float damageOverTime = 125f;
        public float slowPercentage = 0.5f;

        [Header("Unity Setup")]
        public string enemyTag = "Enemy";
        public Transform pivot;

        public Transform firePoint;
        private float time = 0f;
        private float timeInterval = 0.3f;



        void UpdateTarget() 
        {

     
            float shortestDist = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in GameManager.instance.enemyList)
            {
                if (enemy == null) continue;
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(distToEnemy < shortestDist) 
                {
                    shortestDist = distToEnemy;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy && shortestDist <= range) 
            {
                target = closestEnemy.transform;
                targetEnemy = closestEnemy.GetComponent<Enemy>();
            }
            else 
            {
                target = null;
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (time <= 0)
            {
                UpdateTarget();
                time = timeInterval;
            }
            else
            {
                time -= Time.deltaTime;
            }
            FindEnemy();

        }

        private void FindEnemy()
        {
            if (target)
            {
                if (useLaser)
                {
                    Laser();
                }
                else
                {
                    LockOnTarget();
                    if (fireTimer <= 0f)
                    {
                        Shoot();
                        fireTimer = 1f / fireRate;
                    }

                    fireTimer -= Time.deltaTime;

                }
            }
            else
            {
                if (useLaser)
                {
                    if (lineRenderer.enabled)
                    {
                        lineRenderer.enabled = false;
                        impactEffect.Stop();
                        impactLight.enabled = false;
                    }
                }
            }
        }

        void Shoot() 
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.setDamage(cannonDamage);
            SFXManager.Instance.Cannon_Shot();
            if (bullet) 
            {
                bullet.Seek(target);
            }
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        private void LockOnTarget()
        {
            // Lock onto target
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(pivot.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            pivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        void Laser() 
        {
            targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
            targetEnemy.Slow(slowPercentage);
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
                impactEffect.Play();
                impactLight.enabled = true;
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.position);

            Vector3 dir = firePoint.position - target.position;
            impactEffect.transform.position = target.position + dir.normalized * .1f;
            impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
