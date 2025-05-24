using System;
using Core.MatchmakingComponents;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI
{
    public class MatchTimerUI : MonoBehaviour
    {
        [SerializeField] private string defaultTextTime = "--:--";
        [SerializeField] private TMP_Text timerText;
        
        private MatchTimer _matchTimer;

        public void Initialize(MatchTimer matchTimer)
        {
            _matchTimer = matchTimer;
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

        public void Update()
        {
            if(!_matchTimer && _matchTimer.IsRunning) return;
            var remainingTime = _matchTimer.GetRemainingTime();
            
            if(remainingTime != null)
                UpdateTime(remainingTime.Value);
        }
    }
}