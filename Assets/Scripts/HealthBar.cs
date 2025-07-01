using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        public Image fillImage;

        public void SetHealth(float currentHP, float maxHP)
        {
            fillImage.fillAmount = currentHP / maxHP;
        }
    }
}