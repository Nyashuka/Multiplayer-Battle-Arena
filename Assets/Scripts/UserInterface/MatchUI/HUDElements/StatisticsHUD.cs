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
        
        public void UpdateStatistic(PlayerStatisticNetwork playerStatisticNetwork)
        {
            statisticsText.text = $"k: {playerStatisticNetwork.Kills}, d: {playerStatisticNetwork.Deaths}";
        }
        
        private void OnStatisticsChanged(IEventBusArgs args)
        {
            if (args is PlayerStatsChangedEventArgs statisticsChangedEvent)
            {
                var playerStatistic = statisticsChangedEvent.PlayerStatisticNetwork;
                        
                UpdateStatistic(playerStatistic);
            }
        }
                
        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerMatchStatsChanged, OnStatisticsChanged);
        }
                
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerMatchStatsChanged, OnStatisticsChanged);
        }
    }
}