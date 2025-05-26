using Core.MatchmakingComponents.ScoreSystem;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.HUDElements
{
    public class Statistics : HUDElement
    {
        [SerializeField] private TMP_Text statisticsText;
        
        public void UpdateStatistic(PlayerStatistic playerStatistic)
        {
            statisticsText.text = $"k: {playerStatistic.Kills}, d: {playerStatistic.Deaths}";
        }
        
        private void OnStatisticsChanged(IEventBusArgs args)
        {
            if (args is StatisticsChangedEventArgs statisticsChangedEvent)
            {
                var playerStatistic = statisticsChangedEvent
                    .MatchStatistic.GetPlayerStatistic(statisticsChangedEvent.Owner);
                        
                if (playerStatistic != null)
                {
                    UpdateStatistic(playerStatistic);
                }
            }
        }
                
        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StatisticsChanged, OnStatisticsChanged);
        }
                
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.StatisticsChanged, OnStatisticsChanged);
        }
    }
}