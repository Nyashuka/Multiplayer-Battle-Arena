using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons.Abstract
{
    public abstract class Weapon : NetworkBehaviour
    {
        [Networked] public NetworkObject Owner { get; set; }
       
        public override void Spawned()
        {
            if (Owner != null)
            {
                var player = Owner.GetComponent<Player>();
                transform.SetParent(player.GetPrimaryWeaponTransform());
            }
        }
        
        public abstract void Fire(Vector3 direction, NetworkRunner runner, PlayerRef owner);
    }
}