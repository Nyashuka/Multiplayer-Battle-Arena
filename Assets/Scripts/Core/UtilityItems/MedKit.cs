using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.Audio;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.UtilityItems
{
    public class MedKit : NetworkBehaviour
    {
        [Networked] private string Id { get; set; }

        public void Initialize(string id, Transform parent)
        {
            if (HasStateAuthority)
            {
                Id = id;
            }
            
            transform.SetParent(parent, false);
            transform.localPosition = Vector3.zero;
            
            Runner.Despawn(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            var config = (MedKitItemConfig)ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetById(Id);
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