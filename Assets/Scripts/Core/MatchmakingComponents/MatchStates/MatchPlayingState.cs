using Core.MatchmakingComponents;
using Fusion;

namespace Infrastructure.MatchStates
{
    public class MatchPlayingState : IMatchState
    {
        private readonly MatchManager _manager;
        
        public MatchPlayingState(MatchManager manager)
        {
            _manager = manager;     
        }
        
        public void Enter()
        {
            _manager.MatchTimer.StartMatchTimer(180);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        public void OnPlayerDeath(PlayerRef victim, PlayerRef killer)
        {
            _manager.ProcessDeath(victim, killer);
        }

        public MatchStateEnum ToEnum()
        {
            return MatchStateEnum.Playing;
        }
    }
}