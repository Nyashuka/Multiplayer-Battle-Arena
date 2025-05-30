using System.Collections.Generic;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using ScriptableObjects.Weapons;

namespace Services.EventBus.EventBusArguments
{
    public class StartRespawnEventArgs : IEventBusArgs
    {
        public StartRespawnEventArgs(float respawnAt, NetworkRunner runner)
        {
            RespawnAt = respawnAt;
            Runner = runner;
        }

        public float RespawnAt { get; private set; }
        public NetworkRunner Runner { get; private set; }
    }
}