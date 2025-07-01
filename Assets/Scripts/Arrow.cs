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
        private bool _blockTrigger = true;

        private float _damage;
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
            _blockTrigger = false;
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
            if(_blockTrigger)
                return;
            if (other.gameObject == _owner) return;

            var target = other.transform.parent?.GetComponent<Character>();
            if (target != null)
            {
                target.TakeDamage(_damage);
            }
            _achieveTarget = true;
            _blockTrigger = true;
            gameObject.transform.SetParent(other.transform, true);
        }

        private IEnumerator DisableProjectile()
        {
            yield return new WaitForSeconds(lifeTime);
            Deactivate();
        }
        private void Deactivate()
        {
            gameObject.transform.parent = null;
            _blockTrigger = false;
            _achieveTarget = false;
            arowsPool.ReturnObject(this);
        }
    }
}