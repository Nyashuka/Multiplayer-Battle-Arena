using Core.PlayerComponents;
using Core.PlayerComponents.HealthComponent;
using Data;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "UtilityAbilities/MedKit")]
    public class MedKitAbility : UtilityItemAbilityBase
    {
        public override void Use(UtilityItemConfig config, ItemUseContext itemUseContext)
        {
            var medKitConfig = (MedKitItemConfig)config;

            if (itemUseContext.User.TryGetComponent(out NetworkHealth networkHealth))
            {
                networkHealth.Heal(medKitConfig.HealAmount);
            }
        }
    }
}