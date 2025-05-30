using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Core.MatchmakingComponents
{
    public class PlayersRespawner 
    {
        private readonly Dictionary<PlayerRef, TickTimer> _timers = new();
        private readonly IMatchContext _context;
        private readonly float _respawnDelay = 10f;

        public PlayersRespawner(IMatchContext context)
        {
            _context = context;
        }

        public void AddPlayerToRespawn(PlayerRef playerRef, NetworkRunner runner)
        {
            _timers[playerRef] = TickTimer.CreateFromSeconds(runner, _respawnDelay);
        }

        public float GetRespawnAt(PlayerRef playerRef, NetworkRunner runner)
        {
            return GetRemainingTime(playerRef, runner) + runner.SimulationTime;
        }

        private float GetRemainingTime(PlayerRef playerRef, NetworkRunner runner)
        {
            if (!_timers.TryGetValue(playerRef, out var tickTimer)) return 0;

            var elapsed = tickTimer.RemainingTime(runner);
            return elapsed ?? 0;
        }

        public void Update(NetworkRunner runner)
        {
            foreach (var (player, timer) in _timers.ToList())
            {
                if (timer.Expired(runner))
                {
                    _context.RespawnPlayer(player);
                    _timers.Remove(player);
                }
            }
        }
    }
}