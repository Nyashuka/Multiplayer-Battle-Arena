using System;
using Fusion;
using Infrastructure.MatchStates;

namespace Core.MatchmakingComponents.MatchStates
{
    public class MatchWarmupState : IMatchState, IDisposable
    {
        private readonly MatchManager _manager;
        private readonly float _warmupDuration = 5f;

        public MatchWarmupState(MatchManager manager)
        {
            _manager = manager;
        }

        public void Enter()
        {
            _manager.MatchTimer.StartMatchTimer(_warmupDuration);
            _manager.MatchTimer.TimerEndedEvent += OnTimerEnded;
        }

        private void OnTimerEnded()
        {
            _manager.SetState(new MatchPlayingState(_manager));
        }

        public void Exit()
        {
            Dispose();
        }

        public void Update()
        {
        }

        public void OnPlayerDeath(PlayerRef victim, PlayerRef killer)
        {
            _manager.RespawnPlayer(victim);
        }

        public MatchStateEnum ToEnum()
        {
            return MatchStateEnum.Warmup;
        }

        public void Dispose()
        {
            _manager.MatchTimer.TimerEndedEvent -= OnTimerEnded;
        }
    }
}