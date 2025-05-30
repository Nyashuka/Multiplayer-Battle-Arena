using Core.MatchmakingComponents;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;

namespace Infrastructure.MatchStates
{
    public class MatchWarmupState : IMatchState
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
        }

        public void Exit()
        {
        }

        public void Update()
        {
            if (!_manager.MatchTimer.IsRunning)
            {
                _manager.SetState(new MatchPlayingState(_manager));
            }
        }

        public void OnPlayerDeath(PlayerRef victim, PlayerRef killer)
        {
        }

        public MatchStateEnum ToEnum()
        {
            return MatchStateEnum.Warmup;
        }
    }
}