using Core.Modifiers;
using Core.PlayerComponents;
using Core.UtilityItems.Abstract;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.UtilityItems
{
    public class Shield : UtilityItem
    {
        [Networked] private Player OwnerPlayer { get; set; }
        private TickTimer _shieldTimer;
        private IModifier<int> _modifier;
        private ShieldItemConfig _config;
        private bool _isStarted;

        public void Initialize(string itemId, Player ownerPlayer)
        {
            if (HasStateAuthority)
            {
                ItemId = itemId;
            }
            
            OwnerPlayer = ownerPlayer;
            
            _config = (ShieldItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>()
                .GetById(ItemId);
            
            ActivateShield();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_SpawnFX()
        {
            transform.SetParent(OwnerPlayer.transform);
            transform.localPosition = Vector3.zero;
            
            _config = (ShieldItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>()
                .GetById(ItemId);
            
            ServiceLocator.Instance.GetService<VFXService>()
                .PlayLocalVFX(_config.ShieldEffect, 
                    transform.position + transform.up, 
                    Quaternion.identity, 
                    transform.parent);
        }

        private void ActivateShield()
        {
            if (!HasStateAuthority) return;
           
            Rpc_SpawnFX();
            _modifier = new ShieldModifier(_config.DamageReductionMultiplier);
            _shieldTimer = TickTimer.CreateFromSeconds(Runner, _config.Duration);
            _isStarted = true;
            OwnerPlayer.NetworkHealth.AddIncomingDamageModifier(_modifier);
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || !_isStarted) return;

            if (_shieldTimer.Expired(Runner))
            {
                _modifier.SetExpired();
                
                Runner.Despawn(Object);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            
        }
        
        
    }
}