using Core.PlayerComponents;
using Core.UtilityItems;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "UtilityAbilities/MedKit")]
    public class MedKitAbility : UtilityItemAbilityBase
    {
        public override void Use(NetworkRunner runner, Player user, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext)
        {
            var medKitConfig = (MedKitItemConfig)config;

            if(user)
            {
                user.NetworkHealth.Heal(medKitConfig.HealAmount);
                var medKit = runner.Spawn(medKitConfig.MedKitPrefab, 
                    user.transform.position, 
                    Quaternion.identity, 
                    user.Object.InputAuthority)
                    .GetComponent<MedKit>();
                
                medKit.Initialize(medKitConfig.Id, user);
            }
        }
    }
}