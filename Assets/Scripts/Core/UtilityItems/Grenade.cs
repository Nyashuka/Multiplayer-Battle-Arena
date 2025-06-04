using System.Collections.Generic;
using Core.PlayerComponents.HealthComponent;
using Core.UtilityItems.Abstract;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.Audio;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.UtilityItems
{
    public class Grenade : UtilityItem
    {
        private GrenadeItemConfig _config;
        [Networked] private string Id { get; set; }
        private float _timer;
        public PlayerRef Owner { get; private set; }

        public void Initialize(GrenadeItemConfig grenadeConfig, PlayerRef owner)
        {
            if (HasStateAuthority)
            {
                Id = grenadeConfig.Id;
            }
            Owner = owner;
            _config = grenadeConfig;
            _timer = _config.ExplodeDelay;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority || _timer <= 0f)
                return;
            
            _timer -= Runner.DeltaTime;
            if (_timer <= 0f)
            {
                Explode();
            } 
        }

        private void Explode() 
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _config.Range);

            HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

            foreach (var hit in hits)
            {
                IDamagable damagable = null;
                if (hit.TryGetComponent(out IDamagable directHit))
                {
                    damagable = directHit;
                }
                else if (hit.transform.root.TryGetComponent(out IDamagable rootHit))
                {
                    damagable = rootHit;
                }

                if (damagable != null && !damagedTargets.Contains(damagable))
                {
                    damagedTargets.Add(damagable);

                    var damageData = new DamageData()
                    {
                        Attacker = Owner,
                        Damage = _config.Damage
                    };
                    Debug.Log(Owner);
                    damagable.TakeDamage(damageData);
                }
            }

            DestroySelf();
        }
        
        private void DestroySelf()
        {
            Runner.Despawn(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            var itemConfig = 
                (GrenadeItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetById(Id);
            
            ServiceLocator.Instance.GetService<AudioService>()
                .PlaySfx(itemConfig.ExplodeSound, transform.position);
            
            ServiceLocator.Instance.GetService<VFXService>()
                .PlayLocalVFX(itemConfig.ExplodeEffect, transform.position, transform.rotation);
        }
    }
}