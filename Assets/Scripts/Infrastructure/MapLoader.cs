using Environment;
using Fusion;
using UnityEngine;

namespace Infrastructure
{
    public class MapLoader
    {
        private readonly NetworkPrefabRef _mapPrefab;
        private readonly NetworkRunner _runner;
        
        private Map _currentMap;

        public MapLoader(NetworkRunner runner, NetworkPrefabRef mapPrefab)
        {
            _runner = runner;
            _mapPrefab = mapPrefab;
        }

        public Map LoadMap()
        {
            if (_currentMap != null)
                _runner.Despawn(_currentMap.Object);

            _currentMap = _runner.Spawn(_mapPrefab, Vector3.zero, Quaternion.identity).GetComponent<Map>();

            return _currentMap;
        }
    }
}