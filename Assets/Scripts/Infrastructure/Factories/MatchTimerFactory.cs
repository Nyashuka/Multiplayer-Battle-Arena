using Core.MatchmakingComponents;
using Fusion;

namespace Infrastructure.Factories
{
    public class MatchTimerFactory
    {
        private readonly NetworkRunner _runner;
        private readonly NetworkPrefabRef _networkPrefabRef;

        public MatchTimerFactory(NetworkRunner runner, NetworkPrefabRef networkPrefabRef)
        {
            _runner = runner;
            _networkPrefabRef = networkPrefabRef;
        }

        public MatchTimer Create()
        {
            return _runner.Spawn(_networkPrefabRef).GetComponent<MatchTimer>();
        }
    }
}