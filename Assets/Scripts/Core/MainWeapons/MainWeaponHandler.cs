using System.Collections.Generic;
using Core.MainWeapons.Abstract;
using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;

namespace Core.MainWeapons
{
    public class MainWeaponHandler : NetworkBehaviour
    {
        [Networked] private string EquippedWeaponId { get; set; }
        [Networked] private WeaponBase EquippedWeapon { get; set; }

        [SerializeField] private List<WeaponConfigBase> arsenal;
        [SerializeField] private WeaponConfigBase defaultWeapon;

        [SerializeField] private Transform gunHolder;

        public override void Spawned()
        {
            EquippedWeaponId = defaultWeapon.ID;
        }

        public void Fire(Vector3 start, Vector3 direction)
        {
            if (HasInputAuthority)
            {
                if(EquippedWeapon == null) return;
                
                EquippedWeapon.Fire(start, direction);
            }
        }

        public void EquipWeapon(WeaponBase weapon)
        {
            if(!HasStateAuthority) return;

            if (EquippedWeapon != null && EquippedWeapon.Object)
            {
                Runner.Despawn(EquippedWeapon.Object);
            }
            
            EquippedWeapon = weapon;
            EquippedWeaponId = EquippedWeapon.Config.ID;
            Rpc_SetupPrimaryGunVisual();
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_SetupPrimaryGunVisual()
        {
            EquippedWeapon.Initialize(arsenal.Find(x => x.ID == EquippedWeaponId));
            EquippedWeapon.transform.SetParent(gunHolder);
            EquippedWeapon.transform.localPosition = new Vector3(0, 0, 0.5f);
            EquippedWeapon.transform.localRotation = Quaternion.identity;
            
        }
    }
}