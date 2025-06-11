using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Core.MainWeapons.Abstract;
using Core.Projectiles.Abstract;
using Core.Projectiles.Data;
using Fusion;
using Infrastructure.Factories;
using Services.Audio;
using Services.ServiceLocatorModule;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.ObjectPoolUtil;

namespace Core.MainWeapons
{
    public class SimpleKinematicWeapon : WeaponBase
    {
        [SerializeField] private Transform firePoint;

        // client
        private TickTimer LocalCooldownTimer { get; set; }
        private readonly Dictionary<Guid, VisualProjectileBase> _spawnedProjectiles = new();
        
        // host
        [Networked] private TickTimer CooldownTimer { get; set; }
        private ServerProjectileFactory _serverProjectileFactory;
        
        public override void Spawned()
        {
            base.Spawned();
            if (HasStateAuthority)
            {
                _serverProjectileFactory = new ServerProjectileFactory(Runner, _config);
                CooldownTimer = default;
            }
        }

        public override void Fire(Vector3 start, Vector3 direction)
        {
            if (!HasInputAuthority) return;
            
            var projectileParams = new ProjectileParams
            {
                Id = Guid.NewGuid(),
                VisualStart = firePoint.position,
                CameraStart = start,
                Direction = direction,
                Speed = _config.MuzzleVelocity,
                LifeTime = _config.BulletLifeTime,
                Owner = Object.InputAuthority
            };
            
            if (LocalCooldownTimer.ExpiredOrNotRunning(Runner) && CooldownTimer.ExpiredOrNotRunning(Runner))
            {
                StartFire(projectileParams);
            }
        }

        private Vector3 GetTargetPosition(Vector3 start, Vector3 direction, float distance)
        {
            int layerMask = ~LayerMask.GetMask("Player");
            if (Physics.Raycast(start, direction, out RaycastHit hit,
                    distance, layerMask))
            {
                return hit.point;
            }
            
            return start + direction * _config.MaxDistance;
        }

        private VisualProjectileBase SpawnVisualProjectile(ProjectileParams projectileParams)
        {
            
            var visualProjectile = _dummyProjectilesPool.Get(firePoint.position, 
                Quaternion.LookRotation(projectileParams.Direction)); //Instantiate(visualPrefab, firePoint.position, firePoint.rotation);
            Debug.DrawRay(firePoint.position, projectileParams.Direction * 10f, Color.green, 2f); 
            visualProjectile.Init(projectileParams);
            visualProjectile.Launch();
            
            return visualProjectile;
        }
        
        private void StartFire(ProjectileParams projectileParams)
        {
            LocalCooldownTimer = TickTimer.CreateFromSeconds(Runner, _config.FireRate);
            
            projectileParams.Target = 
                GetTargetPosition(
                    projectileParams.CameraStart, 
                    projectileParams.Direction, 
                    _config.MaxDistance
                    );
 
            projectileParams.Direction = (projectileParams.Target - projectileParams.VisualStart).normalized;
            
            if (HasInputAuthority)
            {
                ServiceLocator.Instance.GetService<AudioService>()
                    .PlaySfx(_config.FireSound, firePoint.position);
            }

            _spawnedProjectiles[projectileParams.Id] = SpawnVisualProjectile(projectileParams);
   
            Rpc_ServerFire(projectileParams);
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_ServerFire(ProjectileParams projectileParams, RpcInfo info = default)
        {
            if (!HasStateAuthority) return;

            if (CooldownTimer.ExpiredOrNotRunning(Runner))
            {
                CooldownTimer = TickTimer.CreateFromSeconds(Runner, _config.FireRate);

                _serverProjectileFactory.Create(projectileParams, this, firePoint, Object.InputAuthority);
                
                RPC_SpawnDummyProjectileInOthers(projectileParams);
            }
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnDummyProjectileInOthers(ProjectileParams projectileParams)
        {
            if(HasInputAuthority) return;

            _spawnedProjectiles[projectileParams.Id] = SpawnVisualProjectile(projectileParams);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_DestroyDummyProjectile(Guid id, Vector3 position, RpcInfo info = default)
        {
            if (!_spawnedProjectiles.TryGetValue(id, out var visualProjectile)) return;

            if (visualProjectile)
            {
                visualProjectile.Explode(position);
                _dummyProjectilesPool.Return(visualProjectile);
            }

            _spawnedProjectiles.Remove(id);
        }
    }
}