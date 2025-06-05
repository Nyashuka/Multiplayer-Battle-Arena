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
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchTimerChanged, OnMatchTimerChanged, true);
        }

        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.MatchTimerChanged, OnMatchTimerChanged);
        }
        
        private void OnMatchTimerChanged(IEventBusArgs args)
        {
            if (args is MatchTimerChangedEventArgs timer)
            {
                _matchTimer = timer.MatchTimer;
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