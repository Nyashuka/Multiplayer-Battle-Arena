using System;
using Core.Projectiles.Abstract;
using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;
using Utils.ObjectPoolUtil;

namespace Core.MainWeapons.Abstract
{
    public abstract class WeaponBase : NetworkBehaviour
    {
        private const int PoolSize = 50;
        
        [Networked] public PlayerRef Owner { get; set; }
        
        protected WeaponConfigBase _config;
        public WeaponConfigBase Config => _config;
        
        protected LocalObjectPool<VisualProjectileBase> _dummyProjectilesPool;

        public void Initialize(WeaponConfigBase config)
        {
            _config = config;
            _dummyProjectilesPool = new(PoolSize, _config.ProjectileConfig.DummyProjectilePrefab);
        }
        
        public abstract void Fire(Vector3 start, Vector3 direction);
    }
}