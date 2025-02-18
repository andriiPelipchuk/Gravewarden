using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Melee, Ranged }

public class Enemy : MonoBehaviour
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
        Debug.Log("Attack");
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
}
