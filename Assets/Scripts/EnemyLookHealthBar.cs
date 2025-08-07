using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyLookHealthBar : MonoBehaviour
    {
        private Camera _cam;

        void Start()
        {
            _cam = Camera.main;
        }

        void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        }

    }
}