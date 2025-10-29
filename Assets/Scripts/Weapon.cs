using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {
        private float _currentDamage;
        private Collider _hitboxCollider;
        private Character _owner;
        private Character _recipient;

        private void Awake()
        {
            _hitboxCollider = GetComponent<Collider>();
            _hitboxCollider.enabled = false; 
            _owner = GetComponentInParent<Character>();
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
            if (_owner == other.gameObject.GetComponent<Character>())
                return;
            if (other.gameObject.tag != "Player")
            {
                Damage(false, other);
            }
            else
            {
                Damage(true, other);
            }

        }
        private void Damage(bool isPlayer, Collider other)
        {
            if (isPlayer)
                _recipient = other.transform.parent?.GetComponent<Character>();
            else
                _recipient = other.GetComponent<Character>();
            if (_recipient == null)
                return;
            _recipient.TakeDamage(_currentDamage);
        }
    }
}