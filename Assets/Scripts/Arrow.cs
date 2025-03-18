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
        private bool itIsMove = false;

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
            itIsMove = true;
            transform.LookAt(target);
            direction = (target - transform.position).normalized;

            StartCoroutine(coroutine);
        }
        private void MoveToTarget()
        {
            transform.position += direction * speed * Time.deltaTime;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(!itIsMove)
                return;
            achieveTarget = true;
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
            itIsMove = false;
            achieveTarget = false;
            arowsPool.ReturnObject(this);
        }
    }
}