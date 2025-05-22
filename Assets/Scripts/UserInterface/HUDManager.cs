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
        
        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StatisticsChanged, OnStatisticsChanged);
        }

        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.StatisticsChanged, OnStatisticsChanged);
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

        private void UpdateStatistic(PlayerStatistic playerStatistic)
        {
            statisticText.text = $"k: {playerStatistic.Kills}, d: {playerStatistic.Deaths}";
        }
    }
}