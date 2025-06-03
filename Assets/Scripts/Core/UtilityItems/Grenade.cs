using Core.PlayerComponents.HealthComponent;
using Core.UtilityItems.Abstract;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.Audio;
using Services.ServiceLocator;
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

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_PlayExplodeSound(string id)
        {
            var itemConfig = 
                (GrenadeItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetById(id);
            ServiceLocator.Instance.GetService<AudioService>().PlaySfx(itemConfig.ExplodeSound, transform.position);
        }
        
        private void Explode() 
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _config.Range);

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

                if (damagable != null)
                {
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
            ServiceLocator.Instance.GetService<AudioService>().PlaySfx(itemConfig.ExplodeSound, transform.position);
        }
    }
}