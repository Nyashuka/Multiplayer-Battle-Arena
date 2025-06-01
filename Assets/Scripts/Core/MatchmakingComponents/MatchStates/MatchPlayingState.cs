using System;
using Fusion;
using Infrastructure.MatchStates;

namespace Core.MatchmakingComponents.MatchStates
{
    public class MatchPlayingState : IMatchState, IDisposable
    {
        private readonly IMatchContext _context;
        
        public MatchPlayingState(IMatchContext context)
        {
            _context = context;     
        }
    
        public void Enter()
        {
            _context.MatchTimer.StartMatchTimer(180);
            _context.MatchTimer.TimerEndedEvent += OnTimerEnded;
        }

        private void OnTimerEnded()
        {
            _context.SetState(new MatchEndedState(_context));        
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
            _context.ProcessDeath(victim, killer);

            if (_context.AlivePlayers.Count <= 1)
            {
                _context.SetState(new MatchEndedState(_context));        
            }
        }

        public MatchStateEnum ToEnum()
        {
            return MatchStateEnum.Playing;
        }

        public void Dispose()
        {
            _context.MatchTimer.TimerEndedEvent -= OnTimerEnded;
        }
    }
}