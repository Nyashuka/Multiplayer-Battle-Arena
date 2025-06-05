using Core.PlayerComponents;
using Core.UtilityItems;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "UtilityAbilities/Shield")]
    public class ShieldAbility : UtilityItemAbilityBase
    {
        public override void Use(NetworkRunner runner, Player user, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext)
        {
            var shieldConfig = (ShieldItemConfig)config;
            
            var shield = runner.Spawn(shieldConfig.ShieldPrefab, 
                user.transform.position, 
                Quaternion.identity, 
                user.Object.InputAuthority).GetComponent<Shield>();
            
            shield.Initialize(config.Id, user);
        }
    }
}