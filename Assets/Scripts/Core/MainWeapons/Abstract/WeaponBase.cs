using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;

namespace Core.MainWeapons.Abstract
{
    public abstract class WeaponBase : NetworkBehaviour 
    {
        [Networked] public PlayerRef Owner { get; set; }
        
        protected WeaponConfigBase _config;

        public void Initialize(WeaponConfigBase config)
        {
            _config = config;
        }
        
        public abstract void Fire(Vector3 start, Vector3 direction);
    }
}