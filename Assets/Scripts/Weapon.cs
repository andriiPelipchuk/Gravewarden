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
            if (other.gameObject.tag == gameObject.transform.parent?.tag)
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
            if(isPlayer)
                _owner = other.transform.parent?.GetComponent<Character>();
            else 
                _owner = other.GetComponent<Character>();
            if (_owner == null)
                return;
            _owner.TakeDamage(_currentDamage);
        }
    }
}