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
        public float damage;
        public Transform bow;

        private AIMovement aiMovement;
        private ObjectPool ObjectPool;
        [SerializeField] LayerMask targetMasks;
        [SerializeField] GameObject arrows;

        private bool coroutineIsRunning = false;
        void Start()
        {
            Health = health;
            AttackDamage = damage;
            AttackRange = stopDistance;
            AttackCooldown = coolDown;

            aiMovement = GetComponent<AIMovement>();
            aiMovement.AddParameters(this);
            ObjectPool = arrows.GetComponent<ObjectPool>();
        }

        private void Update()
        {
            Target = FindClouserTarget();
        }
        public override Transform FindClouserTarget()
        {
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
            if (!coroutineIsRunning)
                StartCoroutine(AttackCoroutine());
        }


        private IEnumerator AttackCoroutine()
        {
            coroutineIsRunning = true;
            canAttack = false;
            Debug.Log("Enemy attacks");

            var arrow = ObjectPool.GetObject();

            arrow.transform.position = bow.position;
            arrow.transform.SetParent(bow, true);

            arrow.gameObject.SetActive(true);

            var arrowClass = arrow.GetComponent<Arrow>();
            arrowClass.Init(ObjectPool);

            // Realization attack & adjust cooldown for animations 
            yield return new WaitForSeconds(AttackCooldown);
            arrow.transform.parent = null;
            Shoot(arrowClass);

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