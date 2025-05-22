using Core.MatchmakingComponents.ScoreSystem;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text statisticText;
        
        private PlayerStatistic _playerStatistic;

        public void Start()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StatisticsChanged, OnStatisticsChanged);
        }

        private void OnStatisticsChanged(IEventBusArgs args)
        {
            if (args is StatisticsChangedEventArgs statisticsChangedEvent)
            {
                _playerStatistic = statisticsChangedEvent
                    .MatchStatistic.GetPlayerStatistic(statisticsChangedEvent.Owner);
                UpdateStatistic(_playerStatistic);
            }
        }

        private void UpdateStatistic(PlayerStatistic playerStatistic)
        {
            statisticText.text = $"k: {playerStatistic.Kills}, d: {playerStatistic.Deaths}";
        }
    }
}