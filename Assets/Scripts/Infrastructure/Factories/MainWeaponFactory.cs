using Core.PlayerComponents.MainWeapons.Abstract;
using Fusion;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class MainWeaponFactory
    {
        private readonly NetworkRunner _runner;
        private readonly NetworkPrefabRef _networkPrefabRef;

        public MainWeaponFactory(NetworkRunner runner, NetworkPrefabRef networkPrefabRef)
        {
            _runner = runner;
            _networkPrefabRef = networkPrefabRef;
        }

        public WeaponBase Create(PlayerRef owner, Transform parent, Vector3 localOffset = default)
        {
            WeaponBase weapon = null;
            
            _runner.Spawn(_networkPrefabRef, parent.position, parent.rotation, owner, (runner, obj) => {
                weapon = obj.GetComponent<WeaponBase>();
                weapon.Owner = owner;
                weapon.transform.SetParent(parent);
                weapon.transform.localPosition = localOffset;
                weapon.transform.localRotation = Quaternion.identity;
            });

            return weapon;
        }
    }
}