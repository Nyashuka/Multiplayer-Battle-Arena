using Fusion;
using Infrastructure.MatchStates;
using UnityEngine;

namespace Core.MatchmakingComponents.MatchStates
{
    public class MatchEndedState : IMatchState
    {
        private IMatchContext _context;

        public MatchEndedState(IMatchContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            Debug.Log("Match Ended");
            _context.SendAllStatistic();
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        public void OnPlayerDeath(PlayerRef victim, PlayerRef killer)
        {
        }

        public MatchStateEnum ToEnum()
        {
            return MatchStateEnum.Ending;
        }
    }
}