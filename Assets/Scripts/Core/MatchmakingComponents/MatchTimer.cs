using System;
using Fusion;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class MatchTimer : NetworkBehaviour
    {
        [Networked]
        private TickTimer NetworkTimer { get; set; }

        [SerializeField]
        private float matchDurationSeconds = 180f;

        public event Action<float> TimerUpdatedEvent;
        public event Action TimerEndedEvent;

        [Networked] public bool IsRunning { get; private set; }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority && !IsRunning) return;

            if (NetworkTimer.Expired(Runner))
            {
                IsRunning = false;
                TimerEndedEvent?.Invoke();
            }
            else
            {
                float timeLeft = NetworkTimer.RemainingTime(Runner) ?? 0f;
                TimerUpdatedEvent?.Invoke(timeLeft);
            }
        }

        public void StartMatchTimer()
        {
            if (!HasStateAuthority) return;
            
            NetworkTimer = TickTimer.CreateFromSeconds(Runner, matchDurationSeconds);
            IsRunning = true;
        }

        public float? GetRemainingTime()
        {
            return NetworkTimer.RemainingTime(Runner);
        }
    }
}