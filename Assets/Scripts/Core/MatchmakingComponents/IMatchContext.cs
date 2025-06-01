using System.Collections.Generic;
using Core.MatchmakingComponents.ScoreSystem;
using Core.PlayerComponents;
using Environment;
using Fusion;
using Infrastructure.MatchStates;

namespace Core.MatchmakingComponents
{
    public interface IMatchContext
    {
        MatchTimer MatchTimer { get; }
        Dictionary<PlayerRef, Player> Players { get; }
        List<PlayerRef> AlivePlayers { get; }
        Map Map { get; }
        MatchScore MatchScore { get; }
        MatchStatistic MatchStatistic { get; }

        void SetState(IMatchState newState);
        void RespawnPlayer(PlayerRef playerRef);
        void ProcessDeath(PlayerRef victim, PlayerRef killer);
        void SendAllStatistic();
    }
}