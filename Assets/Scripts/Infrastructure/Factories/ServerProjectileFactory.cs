using Core.MainWeapons;
using Core.Projectiles.Data;
using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class ServerProjectileFactory
    {
        private readonly WeaponConfigBase _config;
        private readonly NetworkRunner _runner;
        
        public ServerProjectileFactory(NetworkRunner runner, WeaponConfigBase config)
        {
            _config = config;
            _runner = runner;
        }
        
        public void Create(ProjectileParams projectileParams, SimpleKinematicWeapon weapon, Transform firePoint, PlayerRef playerRef)
        {
            projectileParams.Speed = _config.MuzzleVelocity;
            projectileParams.Damage = _config.Damage;
            projectileParams.LifeTime = _config.BulletLifeTime;

            if (Physics.Raycast(projectileParams.CameraStart, projectileParams.Direction, out RaycastHit hit,
                    _config.MaxDistance))
            {
                projectileParams.Target = hit.point;
            }
            else
            {
                projectileParams.Target =
                    projectileParams.CameraStart + projectileParams.Direction * _config.MaxDistance;
            }

            var serverPrefab = _config.ProjectileConfig.ServerProjectilePrefab;
            var serverProjectile =
                _runner.Spawn(serverPrefab, firePoint.position, firePoint.rotation, playerRef);
            serverProjectile.Init(projectileParams);
            serverProjectile.weapon = weapon; 
        }
    }
}