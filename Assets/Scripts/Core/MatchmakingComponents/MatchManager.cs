using System.Collections.Generic;
using Data;
using Fusion;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour
    {
        private List<Player> _players;
        
        public override void Spawned()
        {
            foreach (var player in _players)
            {
                var playerHealth = player.NetworkHealth;
                playerHealth.DeathEvent += OnPlayerDeath;
            }     
        }

        private void OnPlayerDeath(DeathData data)
        {
            
        }
    }
}