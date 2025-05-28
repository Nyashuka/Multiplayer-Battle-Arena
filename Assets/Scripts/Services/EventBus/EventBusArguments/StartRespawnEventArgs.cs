using System.Collections.Generic;
using ScriptableObjects.Weapons;

namespace Services.EventBus.EventBusArguments
{
    public class StartRespawnEventArgs : IEventBusArgs
    {
        public StartRespawnEventArgs(float respawnAt, IReadOnlyList<WeaponConfigBase> availableWeapons)
        {
            RespawnAt = respawnAt;
            AvailableWeapons = availableWeapons;
        }

        public float RespawnAt { get; private set; }
        public IReadOnlyList<WeaponConfigBase> AvailableWeapons { get; private set; }
        
    }
}