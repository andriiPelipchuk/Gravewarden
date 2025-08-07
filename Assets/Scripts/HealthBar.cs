using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        public void SetHealth(float currentHP, float maxHP, Image fillImage)
        {
            fillImage.fillAmount = currentHP / maxHP;
        }
    }
}