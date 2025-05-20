using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Environment
{
    public class Map : NetworkBehaviour
    {
        [SerializeField] private List<SpawnPoint> spawnPoints;
        
        public List<SpawnPoint> SpawnPoints => spawnPoints;
    }
}