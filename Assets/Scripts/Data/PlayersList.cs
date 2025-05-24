using System.Collections.Generic;
using Core;
using Fusion;

namespace Data
{
    public class PlayersList
    {
        private Dictionary<PlayerRef, Player> Players { get; set; } = new();

        public Player this[PlayerRef playerRef] => GetPlayer(playerRef);

        public void Add(PlayerRef playerRef, Player player)
        {
            Players[playerRef] = player;
        }

        public Player GetPlayer(PlayerRef playerRef)
        {
            return Players[playerRef];
        }
    }
}