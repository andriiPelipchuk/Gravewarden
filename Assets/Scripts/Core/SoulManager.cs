using UnityEngine;
using TMPro;

namespace Assets.Scripts.Core
{
    public class SoulManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _soulText;
        private int _soulCount;

        public int GetSoulCount()
        {
            return _soulCount;
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
        public void LoadSouls( int souls)
        {
            _soulCount = souls;
            UpdateSoulText();
        }
    }
}