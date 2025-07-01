using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {
        private float _currentDamage;
        private Collider _hitboxCollider;

        private void Awake()
        {
            _hitboxCollider = GetComponent<Collider>();
            _hitboxCollider.enabled = false; 
        }

        public void ActivateHitbox(float damage)
        {
            _currentDamage = damage;
            _hitboxCollider.enabled = true;
        }

        public void DeactivateHitbox()
        {
            _hitboxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.transform.parent?.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(_currentDamage);
            }
        }
    }
}