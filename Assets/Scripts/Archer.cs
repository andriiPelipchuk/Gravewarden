using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Archer : Character
    {

        public float detectionRadius = 10;
        public float health = 50;
        public float stopDistance = 8;
        public float coolDown = 3;
        public float rechange = 3;
        public float damage;
        public Transform bow;

        private AIMovement aiMovement;
        private ObjectPool objectPool;
        [SerializeField] LayerMask targetMasks;

        private float targetUpdateInterval = 0.5f;
        private float targetUpdateTimer = 0;

        private bool coroutineIsRunning = false;
        void Start()
        {
            Health = health;
            AttackDamage = damage;
            AttackRange = stopDistance;
            AttackCooldown = coolDown;

            aiMovement = GetComponent<AIMovement>();
            aiMovement.AddParameters(this);

            objectPool = FindAnyObjectByType<ObjectPool>().GetComponent<ObjectPool>();
            if (objectPool == null)
            {
                Debug.LogError("ObjectPool not found in the scene.");
            }
        }

        private void Update()
        {
            targetUpdateTimer += Time.deltaTime;
            if (targetUpdateTimer >= targetUpdateInterval)
            {
                Target = FindClouserTarget();
                targetUpdateTimer = 0;
            }
        }
        public override Transform FindClouserTarget()
        {
            if (targetMasks == 0)
            {
                Debug.LogWarning("Target masks are not set for the Archer.");
                return null;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMasks);
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = col.transform;
                }
            }

            target = closestTarget;
            return target;
        }

        public override void Attack()
        {
            ChoseAttack();
        }

        private void ChoseAttack()
        {
            if (coroutineIsRunning || !canAttack || Target == null)
                return;

            var distanceToTarget = Vector3.Distance(transform.position, Target.position);
            if (distanceToTarget > 2)
                StartCoroutine(ShootCoroutine());
            else
                StartCoroutine(AttackCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            coroutineIsRunning = true;
            canAttack = false;

            yield return new WaitForSeconds(rechange);

            var arrow = objectPool.GetObject();
            if (arrow == null)
            {
                Debug.LogError("Failed to retrieve an arrow from the object pool.");
                coroutineIsRunning = false;
                canAttack = true;
                yield break;
            }

            arrow.transform.position = bow.position;
            arrow.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            arrow.transform.SetParent(bow.parent, true);
            arrow.gameObject.SetActive(true);

            var arrowClass = arrow.GetComponent<Arrow>();
            arrowClass.Init(objectPool);

            yield return new WaitForSeconds(AttackCooldown);
            arrow.transform.parent = null;

            Shoot(arrowClass);

            canAttack = true;
            coroutineIsRunning = false;
        }
        private IEnumerator AttackCoroutine()
        {
            coroutineIsRunning = true;
            canAttack = false;
            // Realization attack & adjust cooldown for animations 
            yield return new WaitForSeconds(AttackCooldown / 3);
            canAttack = true;
            coroutineIsRunning = false;
        }
        private void Shoot(Arrow arrow)
        {
            arrow.AddTarget(target.position);
        }
        protected override void Die()
        {
            Debug.Log("Enemy has died");
        }
    }
}