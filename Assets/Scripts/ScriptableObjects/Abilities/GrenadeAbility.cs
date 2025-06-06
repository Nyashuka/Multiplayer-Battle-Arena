using Core.PlayerComponents;
using Core.UtilityItems;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "UtilityAbilities/Grenade Ability")]
    public class GrenadeAbility : UtilityItemAbilityBase
    {
        public override void Use(NetworkRunner runner, Player user, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext)
        {
            var grenadeItemConfig = (GrenadeItemConfig)config;
            var spawnPoint = utilityItemUseContext.ThrowFrom;
        
            var prefab = grenadeItemConfig.GrenadePrefab;
            var instance = runner.Spawn(prefab, spawnPoint, Quaternion.identity, 
                onBeforeSpawned: (runner, obj) =>
                {
                    var grenade = obj.GetComponent<Grenade>();
                    grenade.Initialize(grenadeItemConfig, utilityItemUseContext.Owner);
                });
            
            var rigid = instance.GetComponent<Rigidbody>();
            rigid.AddForce(utilityItemUseContext.AimDirection.normalized * grenadeItemConfig.ThrowForce, 
                ForceMode.Impulse);
        }
    }
}