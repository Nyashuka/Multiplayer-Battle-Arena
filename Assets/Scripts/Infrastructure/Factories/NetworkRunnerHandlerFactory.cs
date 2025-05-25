using Fusion;
using Networking;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class NetworkRunnerHandlerFactory
    {
        private readonly MainNetworkRunnerHandler _networkRunnerHandlerPrefab;

        public NetworkRunnerHandlerFactory(MainNetworkRunnerHandler mainNetworkRunnerHandlerPrefab)
        {
            _networkRunnerHandlerPrefab = mainNetworkRunnerHandlerPrefab;
        }

        public MainNetworkRunnerHandler Create()
        {
            return Object.Instantiate(_networkRunnerHandlerPrefab);
        }
    }
}