using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        public Image fillImage;
/*        public Transform targetToFollow; 
        public Vector3 offset = new Vector3(0, 2.5f, 0);
        public bool playerHealthBar = false;

        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (targetToFollow != null)
            {
                transform.position = targetToFollow.position + offset;
                transform.LookAt(transform.position + mainCamera.transform.forward);
            }
        }*/

        public void SetHealth(float current, float max)
        {
            fillImage.fillAmount = current / max;
        }
    }
}