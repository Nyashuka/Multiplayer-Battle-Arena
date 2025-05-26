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
        public override void Use(UtilityItemConfig config, ItemUseContext itemUseContext)
        {
            var grenadeItemConfig = (GrenadeItemConfig)config;
            var spawnPoint = itemUseContext.ThrowFrom;

            var prefab = grenadeItemConfig.GrenadePrefab;
            var instance = itemUseContext.User.Runner.Spawn(prefab, spawnPoint, Quaternion.identity, 
                onBeforeSpawned: (runner, obj) =>
                {
                    var grenade = obj.GetComponent<Grenade>();
                    grenade.Initialize(grenadeItemConfig);
                });
            
            var rigid = instance.GetComponent<Rigidbody>();
            rigid.AddForce(itemUseContext.AimDirection.normalized * grenadeItemConfig.ThrowForce, ForceMode.Impulse);
        }
    }
}