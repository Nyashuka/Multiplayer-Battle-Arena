using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "UtilityAbilities/MedKit")]
    public class MedKitAbility : UtilityItemAbilityBase
    {
        public override void Use(NetworkRunner runner, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext)
        {
            var medKitConfig = (MedKitItemConfig)config;

            // if (utilityItemUseContext.TryGetComponent(out NetworkHealth networkHealth))
            // {
            //     networkHealth.Heal(medKitConfig.HealAmount);
            // }
        }
    }
}