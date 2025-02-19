using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyType { Melee, Ranged }

namespace Assets.Scripts 
{
    public class Enemy : Character
    {
        public EnemyType enemyType;
        public Transform player;
        public float attackRange = 2.0f;
        public float stopDistance = 5.0f;
        public float shootInterval = 2.0f;
        public GameObject projectilePrefab;
        public Transform shootPoint;

        private NavMeshAgent agent;
        private float lastShootTime;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = enemyType == EnemyType.Melee ? attackRange : stopDistance;

            Health = 50;
            Speed = 2f;
            AttackDamage = 5f;
            AttackRange = 1f;
            AttackCooldown = 1.5f;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (enemyType == EnemyType.Melee)
            {
                MeleeBehavior(distanceToPlayer);
            }
            else if (enemyType == EnemyType.Ranged)
            {
                RangedBehavior(distanceToPlayer);
            }
        }

        void MeleeBehavior(float distanceToPlayer)
        {
            if (distanceToPlayer > attackRange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath();
                Attack();
            }
        }

        void RangedBehavior(float distanceToPlayer)
        {
            if (distanceToPlayer > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath();
                if (Time.time - lastShootTime > shootInterval)
                {
                    Shoot();
                    lastShootTime = Time.time;
                }
            }
        }

        void Attack()
        {
            StartCoroutine(AttackCoroutine());
        }

        void Shoot()
        {
            Debug.Log("Shoot!");
            if (projectilePrefab != null && shootPoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
                Vector3 direction = (player.position - shootPoint.position).normalized;
                projectile.GetComponent<Rigidbody>().linearVelocity = direction * 10.0f;
            }
        }
        private IEnumerator AttackCoroutine()
        {
            canAttack = false;
            Debug.Log("Enemy attacks");
            // Realization Attack
            yield return new WaitForSeconds(AttackCooldown);
            canAttack = true;
        }
        protected override void Die()
        {
            Debug.Log("Enemy has died");
        }
    }

}
