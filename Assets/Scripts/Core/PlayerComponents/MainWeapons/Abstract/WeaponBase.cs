using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons.Abstract
{
    public abstract class WeaponBase : NetworkBehaviour 
    {
        [Networked] public PlayerRef Owner { get; set; }
       
        public abstract void Fire(Vector3 start, Vector3 direction);
    }
}