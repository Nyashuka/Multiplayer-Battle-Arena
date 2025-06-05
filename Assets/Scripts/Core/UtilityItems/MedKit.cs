using Core.PlayerComponents;
using Core.UtilityItems.Abstract;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.Audio;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.UtilityItems
{
    public class MedKit : UtilityItem
    {
        [Networked] private Player OwnerPlayer { get; set; }
        
        public void Initialize(string id, Player ownerPlayer)
        {
            if (HasStateAuthority)
            {
                ItemId = id;
            }
            
            OwnerPlayer = ownerPlayer;
            
            Rpc_SpawnFX();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_SpawnFX()
        {
            transform.SetParent(OwnerPlayer.transform, false);
            transform.localPosition = Vector3.zero;
            
            var config = (MedKitItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetById(ItemId);
            ServiceLocator.Instance.GetService<VFXService>()
                .PlayLocalVFX(config.MedKitEffect,
                transform.position + transform.up, 
                transform.rotation,
                transform.parent);
            
            if (Runner.LocalPlayer == OwnerPlayer.Object.InputAuthority)
            {
                ServiceLocator.Instance.GetService<AudioService>()
                    .PlaySfx(config.MedKitSound, transform.position + transform.up);
            }
        }
    }
}