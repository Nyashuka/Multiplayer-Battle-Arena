using System;
using Core.MatchmakingComponents;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UserInterface.MatchUI
{
    public class RespawnScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        private float _respawnAt;
        private NetworkRunner _networkRunner;

        private bool _timeIsExpired = true;
        public event Action CanRespawn;

        public void SetRespawnTime(float respawnAt)
        {
            _respawnAt = respawnAt;
            _timeIsExpired = false;
            _networkRunner = MatchManager.Instance.Runner;
        }

        public void Update()
        {
            if(_timeIsExpired) return;
            
            var timeLeft = _respawnAt - _networkRunner.SimulationTime;

            if (timeLeft <= 0)
            {
                _timeIsExpired = true;
                timerText.text = "00";
                CanRespawn?.Invoke();
                return;
            }

            int secondsLeft = Mathf.CeilToInt(timeLeft);
            timerText.text = secondsLeft.ToString("D2");
        }
    }
}