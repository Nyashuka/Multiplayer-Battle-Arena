using Fusion;

namespace Infrastructure.MatchStates
{
    public interface IMatchState
    {
        void Enter();
        void Exit();
        void Update();
        void OnPlayerDeath(PlayerRef victim, PlayerRef killer);
        MatchStateEnum ToEnum();
    }
}