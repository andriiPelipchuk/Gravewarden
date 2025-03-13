using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Arrow : MonoBehaviour
    {
        public float speed;
        private Vector3 target;

        [SerializeField] LayerMask layers;

        private void Update()
        {
            MoveToTarget();
        }

        public void AddTarget(Vector3 target) 
        {
            this.target = target;
        }
        private void MoveToTarget()
        {
            var direction = (target - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // FIX IT
            transform.LookAt(target);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == layers)
            {
                print("take damage");
            }
            Destroy(gameObject, 2);
        }
    }
}