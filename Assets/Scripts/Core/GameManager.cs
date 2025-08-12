using UnityEngine;

namespace Assets.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SoulManager _soulManager;

        private void OnEnable()
        {
            EventManager.EnemyHasDied += OnEnemyHasDied;
            EventManager.PlayerHasDied += OnPlayerHasDied;
        }
        private void OnDisable()
        {
            EventManager.EnemyHasDied -= OnEnemyHasDied;
            EventManager.PlayerHasDied -= OnPlayerHasDied;
        }

        private void OnPlayerHasDied()
        {
            
        }

        private void OnEnemyHasDied(int amount)
        {
            _soulManager.AddSouls(amount);
        }
    }
}