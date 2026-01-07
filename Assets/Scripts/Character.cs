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

        [Header("Stamina")]
        public float Stamina { get; protected set; }
        public float CurrentStamina { get; protected set; }
        private float _stepOfDecreaseStamina = 19;
        protected float _regenRate;
        private Coroutine staminaRegenCoroutine;

        [Header("Stamina UI")]
        public Image StaminaFill;
        public Bar StaminaBar;

        [Header("Target")]
        public Transform Target {  get; protected set; }

        protected bool canAttack = true;
        protected bool inAnimation = false;
        protected bool zeroStamina = false;

        protected Transform target;
        public GameObject peacefuleTarget;


        [Header("Health UI")]
        public Image HealthFill;
        public Bar bar;
        protected HealthBarManager healthBarManager;

        public virtual void TakeDamage(float amount)
        {
            CurrentHP -= amount;

            if (healthBarManager != null)
                healthBarManager.ShowBar();

            bar.SetBar(CurrentHP, Health, HealthFill);
            if (CurrentHP <= 0)
            {
                if (healthBarManager != null)
                {
                    StopCoroutine(healthBarManager._healthBarEnumerator);
                }
                Die();
            }
        }

        public virtual void BooleanAtackCheck()
        {
            if(inAnimation || zeroStamina)
            {
                canAttack = false;
            }
            else
            {
                canAttack = true;
            }
        }

        public virtual void StaminaUse()
        {
            CurrentStamina -= _stepOfDecreaseStamina;

            bar.SetBar(CurrentStamina, Stamina, StaminaFill);
            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                zeroStamina = true;
                BooleanAtackCheck();
            }
            if (staminaRegenCoroutine != null)
                StopCoroutine(staminaRegenCoroutine);

            staminaRegenCoroutine = StartCoroutine(StaminaRegen());
        }
        IEnumerator StaminaRegen()
        {
            yield return new WaitForSeconds(1f);

            while (CurrentStamina < Stamina)
            {
                CurrentStamina += _regenRate * Time.deltaTime;
                RegenStaminaBar();
                if (zeroStamina && CurrentStamina > 0)
                {
                    zeroStamina = false;
                    BooleanAtackCheck();
                }

                if (CurrentStamina > Stamina)
                    CurrentStamina = Stamina;

                yield return null;
            }

            staminaRegenCoroutine = null;
        }
        protected virtual void RegenHealPoints()
        {
            bar.SetBar(CurrentHP, Health, HealthFill);
        }
        private void RegenStaminaBar()
        {
            StaminaBar.SetBar(CurrentStamina, Stamina, StaminaFill);
        }
        public virtual Transform FindClouserTarget() { return target; }
        public abstract void Attack();
        protected abstract void Die();
    }
}