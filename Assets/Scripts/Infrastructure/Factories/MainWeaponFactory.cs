using Core.MainWeapons.Abstract;
using Core.PlayerComponents.MainWeapons.Abstract;
using Fusion;
using ScriptableObjects.Weapons;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class MainWeaponFactory
    {
        private readonly NetworkRunner _runner;
        private readonly WeaponConfigBase _weaponConfig;

        public MainWeaponFactory(NetworkRunner runner, WeaponConfigBase weaponConfig)
        {
            _runner = runner;
            _weaponConfig = weaponConfig;
        }

        public WeaponBase Create(PlayerRef owner, Transform parent, Vector3 localOffset = default)
        {
            WeaponBase weapon = null;
            
            _runner.Spawn(_weaponConfig.WeaponPrefabRef, parent.position, parent.rotation, owner, (runner, obj) => {
                weapon = obj.GetComponent<WeaponBase>();
                weapon.Owner = owner;
                weapon.transform.SetParent(parent);
                weapon.transform.localPosition = localOffset;
                weapon.transform.localRotation = Quaternion.identity;
                weapon.Initialize(_weaponConfig);
            });

            return weapon;
        }
    }
}