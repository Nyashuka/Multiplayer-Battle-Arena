using Fusion;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class NetworkRunnerFactory
    {
        private readonly NetworkRunner _networkRunnerPrefab;

        public NetworkRunnerFactory(NetworkRunner networkRunnerPrefab)
        {
            _networkRunnerPrefab = networkRunnerPrefab;
        }

        public NetworkRunner Create()
        {
            return Object.Instantiate(_networkRunnerPrefab);
        }
    }
}