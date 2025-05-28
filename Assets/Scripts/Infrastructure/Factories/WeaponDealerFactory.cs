using Core.MatchmakingComponents;
using Fusion;

namespace Infrastructure.Factories
{
    public class WeaponDealerFactory
    {
        private readonly NetworkPrefabRef _weaponDealerPrefab;
        private readonly NetworkRunner _runner;

        public WeaponDealerFactory(NetworkRunner runner, NetworkPrefabRef weaponDealerPrefab)
        {
            _weaponDealerPrefab = weaponDealerPrefab;
            _runner = runner;
        }

        public WeaponDealer Create()
        {
            return _runner.Spawn(_weaponDealerPrefab).GetComponent<WeaponDealer>();
        }
    }
}