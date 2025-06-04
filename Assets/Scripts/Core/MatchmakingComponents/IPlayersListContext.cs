using System.Collections.Generic;
using Core.PlayerComponents;
using Fusion;
using Services.ServiceLocatorModule.Abstract;

namespace Core.MatchmakingComponents
{
    public interface IPlayersListContext : IService
    {
        public Dictionary<PlayerRef, Player> Players { get; } 
    }
}