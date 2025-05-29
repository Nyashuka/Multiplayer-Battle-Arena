using System.Collections.Generic;
using ScriptableObjects.AdditionWeapons;
using ScriptableObjects.Weapons;

namespace Services.EventBus.EventBusArguments
{
    public class StartRespawnEventArgs : IEventBusArgs
    {
        public StartRespawnEventArgs(float respawnAt, IReadOnlyList<WeaponConfigBase> availableWeapons, IReadOnlyList<UtilityItemConfig> availableUtilityItems)
        {
            RespawnAt = respawnAt;
            AvailableWeapons = availableWeapons;
            AvailableUtilityItems = availableUtilityItems;
        }

        public float RespawnAt { get; private set; }
        public IReadOnlyList<WeaponConfigBase> AvailableWeapons { get; private set; }
        public IReadOnlyList<UtilityItemConfig> AvailableUtilityItems { get; private set; }
        
    }
}