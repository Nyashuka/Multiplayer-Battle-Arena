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
        public void Initialize(string id, Transform parent)
        {
            if (HasStateAuthority)
            {
                ItemId = id;
            }
            
            transform.SetParent(parent, false);
            transform.localPosition = Vector3.zero;
            
            Runner.Despawn(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            var config = (MedKitItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetById(ItemId);
            ServiceLocator.Instance.GetService<VFXService>()
                .PlayLocalVFX(config.MedKitEffect, transform.position + transform.up, transform.rotation, transform.parent);
            if (HasInputAuthority)
            {
                ServiceLocator.Instance.GetService<AudioService>()
                    .PlaySfx(config.MedKitSound, transform.position + transform.up);
            }
        }
    }
}