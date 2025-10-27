using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SoulManager _soulManager;
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private Player _player;
        [SerializeField] private List<Checkpoint> _checkpoints;

        private Checkpoint _currentCheckpoint;

        private void OnEnable()
        {
            EventManager.EnemyHasDied += OnEnemyHasDied;
            EventManager.PlayerHasDied += Respawn;
            EventManager.CheckpointReached += SaveGame;
        }
        private void OnDisable()
        {
            EventManager.EnemyHasDied -= OnEnemyHasDied;
            EventManager.PlayerHasDied -= Respawn;
            EventManager.CheckpointReached -= SaveGame;
        }

        private void Start()
        {
            LoadGame();
        }

        private void SaveGame(int pointID)
        {
            _saveManager.SaveGame(pointID, _soulManager.GetSoulCount());
            Respawn();
            LoadGame();
        }
        private void LoadGame()
        {
            var saveData = _saveManager.LoadGame();
            if (saveData == null)
            {
                _saveManager.SaveGame(0, 0);
                _soulManager.LoadSouls(0);
                Debug.LogWarning("❌ Збереження не знайдено або пошкоджене.");
                return;
            }
            _soulManager.LoadSouls(saveData.soulsData);

            foreach (var checkpoint in _checkpoints)
            {
                if (checkpoint.checkpointId == saveData.checkpoint)
                {
                    _currentCheckpoint = checkpoint;
                    break;
                }
            }
            _player.transform.position = _currentCheckpoint.transform.position;
        }
        private void Respawn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnEnemyHasDied(int amount)
        {
            _soulManager.AddSouls(amount);
        }
    }
}