using Environment;
using Fusion;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class MapFactory
    {
        private readonly NetworkPrefabRef _mapPrefab;
        private readonly NetworkRunner _runner;
        
        public MapFactory(NetworkRunner runner, NetworkPrefabRef mapPrefab)
        {
            _runner = runner;
            _mapPrefab = mapPrefab;
        }

        public Map Create()
        {
            return _runner.Spawn(_mapPrefab, Vector3.zero, Quaternion.identity).GetComponent<Map>();
        }
    }
}