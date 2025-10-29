using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Assets.Scripts
{
    public class Arrow : MonoBehaviour
    {
        public float speed;
        public float lifeTime;

        private ObjectPool arowsPool;

        private Vector3 _direction;
        private bool _achieveTarget = false;

        private float _damage;
        private Character _character;

        private GameObject _owner;

        private IEnumerator coroutine;

        private void Start()
        {
            coroutine = DisableProjectile();
        }

        private void Update()
        {
            if(!_achieveTarget) 
                MoveToTarget();
        }
        public void SetData(float damage, GameObject owner)
        {
            _damage = damage;
            _owner = owner;
        }
        public void Init(ObjectPool objectPool)
        {
            arowsPool = objectPool;
        }
        public void AddTarget(Vector3 target) 
        {
            transform.LookAt(target);
            _direction = (target - transform.position).normalized;

            StartCoroutine(coroutine);
        }
        private void MoveToTarget()
        {
            transform.position += _direction * speed * Time.deltaTime;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner) return;

            if (other.gameObject.tag != "Player")
            {
                Damage(false, other);
            }
            else
            {
                Damage(true, other);
            }
            _achieveTarget = true;
            gameObject.transform.SetParent(other.transform, true);
        }
        private void Damage(bool isPlayer, Collider other)
        {
            if (isPlayer)
                _character = other.transform.parent?.GetComponent<Character>();
            else
                _character = other.GetComponent<Character>();
            if (_character == null)
                return;
            _character.TakeDamage(_damage);
            print($"{_character.name} took {_damage} damage from arrow.");
        }

        private IEnumerator DisableProjectile()
        {
            yield return new WaitForSeconds(lifeTime);
            Deactivate();
        }
        private void Deactivate()
        {
            gameObject.transform.parent = null;
            _achieveTarget = false;
            arowsPool.ReturnObject(this);
        }
    }
}