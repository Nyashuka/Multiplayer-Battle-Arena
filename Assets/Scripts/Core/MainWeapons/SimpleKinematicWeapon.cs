using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Core.MainWeapons.Abstract;
using Core.Projectiles.Abstract;
using Core.Projectiles.Data;
using Fusion;
using Infrastructure.Factories;
using UnityEngine;

namespace Core.MainWeapons
{
    public class SimpleKinematicWeapon : WeaponBase
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private AudioSource audioSource;

        private readonly Dictionary<Guid, VisualProjectileBase> _spawnedProjectiles = new();
        private ServerProjectileFactory _serverProjectileFactory;

        [Networked] private TickTimer CooldownTimer { get; set; }
        private TickTimer LocalCooldownTimer { get; set; }
        
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
                ServerStart = start,
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

        private void StartFire(ProjectileParams projectileParams)
        {
            LocalCooldownTimer = TickTimer.CreateFromSeconds(Runner, _config.FireRate);
            
            if (Physics.Raycast(projectileParams.ServerStart, projectileParams.Direction, out RaycastHit hit,
                    _config.MaxDistance))
            {
                projectileParams.Target = hit.point;
            }
            else
            {
                projectileParams.Target =
                    projectileParams.ServerStart + projectileParams.Direction * _config.MaxDistance;
            }
 
            var visualPrefab = _config.ProjectileConfig.DummyProjectilePrefab;
            var visualProjectile = Instantiate(visualPrefab, firePoint.position, firePoint.rotation);
            visualProjectile.Init(projectileParams);
            visualProjectile.Launch();

            if (HasInputAuthority)
                audioSource.Play();

            _spawnedProjectiles[projectileParams.Id] = visualProjectile;   
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
            
            var visualPrefab = _config.ProjectileConfig.DummyProjectilePrefab;
            var visualProjectile = Instantiate(visualPrefab, firePoint.position, firePoint.rotation);
            visualProjectile.Init(projectileParams);
            visualProjectile.Launch();
            
            _spawnedProjectiles[projectileParams.Id] = visualProjectile;
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_DestroyDummyProjectile(Guid id, Vector3 position, RpcInfo info = default)
        {
            if (!_spawnedProjectiles.TryGetValue(id, out var visualProjectile)) return;

            if (visualProjectile)
            {
                visualProjectile.Explose(position);
                Destroy(visualProjectile.gameObject);
            }

            _spawnedProjectiles.Remove(id);
        }
    }
}