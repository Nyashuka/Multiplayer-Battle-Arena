using Core.MatchmakingComponents.ScoreSystem;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.HUDElements
{
    public class StatisticsHUD : HUDElement
    {
        [SerializeField] private TMP_Text statisticsText;
        
        public void UpdateStatistic(PlayerStatistic playerStatistic)
        {
            statisticsText.text = $"k: {playerStatistic.Kills}, d: {playerStatistic.Deaths}";
        }
        
        private void OnStatisticsChanged(IEventBusArgs args)
        {
            if (args is PlayerStatsChangedEventArgs statisticsChangedEvent)
            {
                var playerStatistic = statisticsChangedEvent.PlayerStatistic;
                        
                UpdateStatistic(playerStatistic);
            }
        }
                
        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerStatsChanged, OnStatisticsChanged);
        }
                
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerStatsChanged, OnStatisticsChanged);
        }
    }
}