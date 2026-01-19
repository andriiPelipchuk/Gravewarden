using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        public enum StatsActions 
        {
            saveStats = 0,
            vigor = 1,
            resistance = 2,
            stamina = 3,
            strenght = 4,
            numberOfSummons = 5,
            strenghtOfSummons = 6,
            closeStats = 7
        }

        public Stats stats;

        public TextMeshProUGUI characterLevelText;
        public TextMeshProUGUI vigorLevelText;
        public TextMeshProUGUI resistanceLevelText;
        public TextMeshProUGUI staminaLevelText;
        public TextMeshProUGUI strenghtLevelText;
        public TextMeshProUGUI numberOfSummonsLevelText;
        public TextMeshProUGUI strenghtOfSummonsLevelText;


        private int _characterLevel = 1;
        private int _vigor = 1;
        private int _resistance = 1;
        private int _stamina = 1;
        private int _strenght = 1;
        private int _numberOfSummons = 1;
        private int _strenghtOfSummons = 1;

        private void RaiseLevel()
        {
            _characterLevel++;
            characterLevelText.text = _characterLevel.ToString();
        }

        private void ChangeStats(ref int value , TextMeshProUGUI textValue)
        {
            value++;
            textValue.text = value.ToString();
            RaiseLevel();
        }

        public void ConvertLevelToParameters(int actionIndex)
        {
            StatsActions selectAction = (StatsActions)actionIndex;
            switch(selectAction)
            {
                case StatsActions.vigor:
                    ChangeStats(ref _vigor, vigorLevelText);
                    break;
                case StatsActions.resistance:
                    ChangeStats(ref _resistance, resistanceLevelText);
                    break;
                case StatsActions.stamina:
                    ChangeStats(ref _stamina, staminaLevelText);
                    break;
                case StatsActions.strenght:
                    ChangeStats(ref _strenght, strenghtLevelText);
                    break;
                case StatsActions.numberOfSummons:
                    if (_numberOfSummons >= 4)
                    {
                        Debug.Log("The number of summons can`t be biggest ");
                        return;
                    }
                    ChangeStats(ref _numberOfSummons, numberOfSummonsLevelText);
                    break;
                case StatsActions.strenghtOfSummons:
                    ChangeStats(ref _strenghtOfSummons, numberOfSummonsLevelText);
                    break;
                case StatsActions.saveStats:
                    break;
                case StatsActions.closeStats:
                    // if don`t press save return default value
                    break;
                default:
                    break;
            }

        }
        
        public int GetStats()
        {
            return _characterLevel;
        }
    }
}