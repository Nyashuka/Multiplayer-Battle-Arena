using System.Collections.Generic;
using ScriptableObjects.AdditionWeapons;
using ScriptableObjects.Weapons;

namespace Services.EventBus.EventBusArguments
{
    public class StartRespawnEventArgs : IEventBusArgs
    {
        public StartRespawnEventArgs(float respawnAt)
        {
            RespawnAt = respawnAt;
        }

        public float RespawnAt { get; private set; }
    }
}