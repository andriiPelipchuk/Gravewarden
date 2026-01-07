using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class HealthBarManager : MonoBehaviour
    {
        [SerializeField] Bar _healthBar;
        public IEnumerator _healthBarEnumerator;

        public int cooldown = 10;

        private void OnEnable()
        {
            _healthBarEnumerator = HideHealthBar();
        }

        public void ShowBar()
        {
            StopCoroutine(_healthBarEnumerator);
            _healthBar.gameObject.SetActive(true);
            StartCoroutine(_healthBarEnumerator);
        }
        
        private IEnumerator HideHealthBar()
        {
            yield return new WaitForSeconds(cooldown);
            _healthBar.gameObject.SetActive(false);
        }
    }
}