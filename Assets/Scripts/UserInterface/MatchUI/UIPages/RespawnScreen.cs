using System;
using Core.MatchmakingComponents;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    public class RespawnScreen : UIPage
    {
        [SerializeField] private TMP_Text timerText;
        
        private bool _timeIsExpired = true;
        private float _respawnAt;
        private NetworkRunner _networkRunner;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStarted, OnMatchStarted);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StartRespawn, OnStartRespawn);
        }
        
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.MatchStarted, OnMatchStarted);
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.StartRespawn, OnStartRespawn);
        }

        private void OnMatchStarted(IEventBusArgs e)
        {
            if (e is MatchStartedEventArgs matchStartedEventArgs)
            {
                _networkRunner = matchStartedEventArgs.NetworkRunner;
            }
        }

        private void OnStartRespawn(IEventBusArgs args)
        {
            if (args is StartRespawnEventArgs startRespawnEventArgs)
            {
                SetRespawnTime(startRespawnEventArgs.RespawnAt);
            }
        }

        private void SetRespawnTime(float respawnAt)
        {
            _respawnAt = respawnAt;
            _timeIsExpired = false;
        }

        public void Update()
        {
            if(_timeIsExpired) return;
            
            var timeLeft = _respawnAt - _networkRunner.SimulationTime;

            if (timeLeft <= 0)
            {
                _timeIsExpired = true;
                timerText.text = "00";
                return;
            }

            int secondsLeft = Mathf.CeilToInt(timeLeft);
            timerText.text = secondsLeft.ToString("D2");
        }
    }
}