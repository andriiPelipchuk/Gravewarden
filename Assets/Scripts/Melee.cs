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

        public Weapon weapon;

        public Animator _animator;

        private AIMovement aiMovement;
        [SerializeField] LayerMask targetMasks;
        void Start()
        {
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
            if (canAttack)
                StartCoroutine(AttackCoroutine());
        }


        private IEnumerator AttackCoroutine()
        {
            canAttack = false;
            if (_animator != null)
            {
                weapon.ActivateHitbox(AttackDamage);
                _animator.SetTrigger("Attack");
            }
            yield return new WaitForSeconds(AttackCooldown);
            weapon.DeactivateHitbox();
            canAttack = true;
        }
        protected override void Die()
        {
            Debug.Log("Enemy has died");
        }

    }
}