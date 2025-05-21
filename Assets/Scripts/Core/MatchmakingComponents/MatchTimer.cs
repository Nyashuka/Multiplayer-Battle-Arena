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

        public bool HasEnded { get; private set; }
        
        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                StartMatchTimer();
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasEnded) return;

            if (NetworkTimer.Expired(Runner))
            {
                HasEnded = true;
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
            HasEnded = false;
        }

        public float? GetRemainingTime()
        {
            return NetworkTimer.RemainingTime(Runner);
        }
    }
}