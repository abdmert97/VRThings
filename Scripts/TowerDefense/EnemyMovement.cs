using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        private Enemy enemy;

        private Transform target;
        private int wavepointIndex = 0;

        void Start()
        {
            enemy = GetComponent<Enemy>();
            target = Waypoints.points[0];
        }
        void FixedUpdate()
        {
            //Vector3 direction = target.position - gameObject.transform.position;
            // gameObject.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            transform.position = Vector3.MoveTowards(transform.position, target.position, enemy.speed * Time.fixedDeltaTime);
            transform.LookAt(target.position);
            if (Vector3.Distance(transform.position, target.position) <= enemy.speed / 100)
            {
                GetNextWaypoint();
            }
            // Reset enemy speed to handle of slow effects
            enemy.speed = enemy.startSpeed;
        }
        void GetNextWaypoint()
        {
            if (wavepointIndex >= Waypoints.points.Length - 1)
                exitMap();
            else
            {
                // Select second waypoint randomly 
                if (wavepointIndex == 0)
                {
                    wavepointIndex = Random.Range(1, 3);
                    target = Waypoints.points[wavepointIndex];
                    return;
                }
                if (wavepointIndex == 1)
                {
                    wavepointIndex++;
                }
                wavepointIndex++;
                target = Waypoints.points[wavepointIndex];
            }
        }
        void exitMap()
        {
            PlayerStatus.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
