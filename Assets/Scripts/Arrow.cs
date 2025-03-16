using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Arrow : MonoBehaviour
    {
        public float speed;
        public float lifeTime;

        private ObjectPool arowsPool;

        private Vector3 direction;
        private bool achieveTarget = false;

        private IEnumerator coroutine;

        private void Start()
        {
            coroutine = DisableProjectile();
        }

        private void Update()
        {
            if(!achieveTarget) 
                MoveToTarget();
        }
        public void Init(ObjectPool objectPool)
        {
            arowsPool = objectPool;
        }
        public void AddTarget(Vector3 target) 
        {
            transform.LookAt(target);
            direction = (target - transform.position).normalized;

            StartCoroutine(coroutine);
        }
        private void MoveToTarget()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            achieveTarget = true;
            gameObject.transform.SetParent(collision.transform, true);
            StopCoroutine(coroutine);
            lifeTime /= 3;
            StartCoroutine(coroutine);
        }

        private IEnumerator DisableProjectile()
        {
            yield return new WaitForSeconds(lifeTime);
            Deactivate();
        }
        private void Deactivate()
        {
            gameObject.transform.parent = null;
            achieveTarget = false;
            var collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
            arowsPool.ReturnObject(this);
        }
    }
}