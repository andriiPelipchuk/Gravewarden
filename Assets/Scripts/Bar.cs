using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Bar : MonoBehaviour
    {
        public void SetBar(float current, float max, Image fillImage)
        {
            fillImage.fillAmount = current / max;
        }
    }
}