using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        public float Health { get; protected set; }
        public float CurrentHP { get; protected set; }
        public int Amount { get; protected set; }
        public float Speed { get; protected set; }
        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackCooldown { get; protected set; }
        public Transform Target {  get; protected set; }

        protected bool canAttack = true;

        protected Transform target;
        public GameObject peacefuleTarget;

        public Image HealthFill;

        public HealthBar HealthBar;

        public virtual void TakeDamage(float amount)
        {
            CurrentHP -= amount;
            HealthBar.SetHealth(CurrentHP, Health, HealthFill);
            if (CurrentHP <= 0)
            {
                Die();
            }
        }
        public virtual Transform FindClouserTarget() { return target; }
        public abstract void Attack();
        protected abstract void Die();
    }
}