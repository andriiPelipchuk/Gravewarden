using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Vampirism : MonoBehaviour
    {
        public Character _character;

        private void OnTriggerEnter(Collider other)
        {
            _character.HealCharacter();
        }
    }
}