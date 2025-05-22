using Core.MatchmakingComponents;
using Fusion;

namespace Infrastructure.Factories
{
    public class MatchManagerFactory
    {
        private readonly NetworkRunner _runner;
        private readonly NetworkPrefabRef _matchManagerPrefab;

        public MatchManagerFactory(NetworkRunner runner, NetworkPrefabRef matchManagerPrefab)
        {
            _runner = runner;
            _matchManagerPrefab = matchManagerPrefab;
        }

        public MatchManager Create()
        {
            return _runner.Spawn(_matchManagerPrefab).GetComponent<MatchManager>();
        }
    }
}