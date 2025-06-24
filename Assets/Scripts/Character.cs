using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        public float Health { get; protected set; }
        protected float currentHP;
        public float Speed { get; protected set; }
        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackCooldown { get; protected set; }
        public Transform Target {  get; protected set; }

        protected bool canAttack = true;

        protected Transform target;
        public GameObject peacefuleTarget;

        public HealthBar HealthBar;

        public virtual void TakeDamage(float amount)
        {
            currentHP -= amount;
            HealthBar.SetHealth(currentHP, Health);
            if (currentHP <= 0)
            {
                Die();
            }
        }
        public virtual Transform FindClouserTarget() { return target; }
        public abstract void Attack();
        protected abstract void Die();
    }
}