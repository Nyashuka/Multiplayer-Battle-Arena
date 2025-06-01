using System;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class MatchTimer : NetworkBehaviour
    {
        [Networked]
        private TickTimer NetworkTimer { get; set; }

        public event Action<float> TimerUpdatedEvent;
        public event Action TimerEndedEvent;

        [Networked] public bool IsRunning { get; private set; }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || !IsRunning) return;

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

        public void StartMatchTimer(float durationSeconds)
        {
            if (!HasStateAuthority) return;
            
            NetworkTimer = TickTimer.CreateFromSeconds(Runner, durationSeconds);
            IsRunning = true;
            
            Rpc_MatchTimerChanged();
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_MatchTimerChanged()
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.MatchTimerChanged, 
                new MatchTimerChangedEventArgs(Runner, this), true);
        }

        public float? GetRemainingTime()
        {
            return NetworkTimer.RemainingTime(Runner);
        }
    }
}