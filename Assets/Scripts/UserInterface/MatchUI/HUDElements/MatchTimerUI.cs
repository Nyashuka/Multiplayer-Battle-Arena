using System;
using System.Collections;
using Core.MatchmakingComponents;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.HUDElements
{
    public class MatchTimerUI : HUDElement
    {
        [SerializeField] private string defaultTextTime = "--:--";
        [SerializeField] private TMP_Text timerText;
        
        private MatchTimer _matchTimer;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStarted, OnMatchStarted, true);
        }

        private void OnMatchStarted(IEventBusArgs e)
        {
            if (e is MatchStartedEventArgs matchStartedEventArgs)
            {
                _matchTimer = matchStartedEventArgs.MatchTimer;
                StartCoroutine(Timer());
            }
        }

        private void Start()
        {
            timerText.text = defaultTextTime;
        }

        public void UpdateTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }

        private IEnumerator Timer()
        {
            while (_matchTimer != null && _matchTimer.IsRunning)
            {
                var remainingTime = _matchTimer.GetRemainingTime();
            
                if(remainingTime != null)
                    UpdateTime(remainingTime.Value);

                yield return new WaitForSeconds(1);
            }     
        }
    }
}