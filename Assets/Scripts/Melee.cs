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
        public int amount;
        public HealthBarManager hpManager;

        public Weapon weapon;

        public Animator _animator;

        private AIMovement aiMovement;
        [SerializeField] LayerMask targetMasks;
        void Start()
        {
            Amount = amount;
            Health = health;
            CurrentHP = Health;
            AttackDamage = damage;
            AttackRange = stopDistance;
            AttackCooldown = coolDown;
            healthBarManager = hpManager;

            aiMovement = GetComponent<AIMovement>();
            aiMovement.AddParameters(this);
        }

        private void Update()
        {
            Target = FindClouserTarget();
        }
        public override Transform FindClouserTarget()
        {
            if (targetMasks == 0)
            {
                Debug.LogWarning("Target masks are not set for the Archer.");
                return null;
            }
            if (Target != null)
            {
                float distance = Vector3.Distance(transform.position, Target.position);
                var targetCharacter = Target.GetComponent<Character>();

                if (distance <= detectionRadius && targetCharacter != null && targetCharacter.CurrentHP > 0)
                {
                    return Target;
                }
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMasks);
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider col in colliders)
            {
                var character = col.GetComponent<Character>();

                if (col.gameObject.tag == "Player")
                {
                    character = col.GetComponentInParent<Character>();
                }
                if (character == null || character.gameObject.layer == gameObject.layer)
                    continue;
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
            EventManager.TriggerEnemyHasDied(Amount);
            gameObject.SetActive(false);
        }

    }
}