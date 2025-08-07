using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class Melee : Character
    {
        public float detectionRadius = 4;
        public float health = 50;
        public float stopDistance = 2;
        public float coolDown = 1;
        public float damage;
        public Image healthFill;

        public Weapon weapon;

        private AIMovement aiMovement;
        [SerializeField] LayerMask targetMasks;

        private bool coroutineIsRunning = false;
        void Start()
        {
            HealthFill = healthFill;
            Health = health;
            CurrentHP = Health;
            AttackDamage = damage;
            AttackRange = stopDistance;
            AttackCooldown = coolDown;

            aiMovement = GetComponent<AIMovement>();
            aiMovement.AddParameters(this);
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
            if(peacefuleTarget)
                return;
            if (!coroutineIsRunning)
                StartCoroutine(AttackCoroutine());
        }


        private IEnumerator AttackCoroutine()
        {
            weapon.ActivateHitbox(AttackDamage);
            coroutineIsRunning = true;
            canAttack = false;
            Debug.Log(gameObject.name + "Attacks");
            // Realization attack & adjust cooldown for animations 
            yield return new WaitForSeconds(AttackCooldown);
            weapon.DeactivateHitbox();
            canAttack = true;
            coroutineIsRunning = false;
        }
        protected override void Die()
        {
            Debug.Log("Enemy has died");
        }

    }
}