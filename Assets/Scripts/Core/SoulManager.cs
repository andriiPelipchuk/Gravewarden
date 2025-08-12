using UnityEngine;
using TMPro;

namespace Assets.Scripts.Core
{
    public class SoulManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _soulText;
        private int _soulCount;

        private void Start()
        {
            _soulCount = 0;
            UpdateSoulText();
        }
        public void AddSouls(int amount)
        {
            _soulCount += amount;
            UpdateSoulText();
        }
        private void UpdateSoulText()
        {
            _soulText.text = _soulCount.ToString();
    }
    }
}