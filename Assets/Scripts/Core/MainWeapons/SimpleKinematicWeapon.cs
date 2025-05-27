using System.Collections.Generic;
using Core.MainWeapons.Abstract;
using Core.Projectiles;
using Core.Projectiles.Abstract;
using Core.Projectiles.Data;
using Core.Projectiles.SmoothedProjectile;
using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;

namespace Core.MainWeapons
{
    public class SimpleKinematicWeapon : WeaponBase
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private AudioSource audioSource;

        private readonly Dictionary<NetworkId, VisualProjectileBase> _visualProjectiles = new();

        public override void Fire(Vector3 start, Vector3 direction)
        {
            if (!HasInputAuthority) return;

            var projectileParams = new ProjectileParams
            {
                VisualStart = firePoint.position,
                ServerStart = firePoint.position,
                Direction = direction,
                Owner = Object.InputAuthority
            };

            RPC_RequestFire(projectileParams);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_RequestFire(ProjectileParams projectileParams, RpcInfo info = default)
        {
            if (!HasStateAuthority) return;

            projectileParams.Speed = _config.MuzzleVelocity;
            projectileParams.Damage = _config.Damage;
            projectileParams.LifeTime = _config.BulletLifeTime;

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

            var serverPrefab = _config.ProjectileConfig.ServerProjectilePrefab;
            var serverProjectile =
                Runner.Spawn(serverPrefab, firePoint.position, firePoint.rotation, Object.InputAuthority,
                    ((runner, o) =>
                    {
                    } ));
            serverProjectile.Init(projectileParams);
            serverProjectile.weapon = this;

            RPC_SpawnDummyProjectile(serverProjectile.Object.Id, projectileParams);
        }
    

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnDummyProjectile(NetworkId serverProjectileId, ProjectileParams projectileParams, RpcInfo info = default)
        {
            var visualPrefab = _config.ProjectileConfig.DummyProjectilePrefab;
            var visualProjectile = Instantiate(visualPrefab, firePoint.position, firePoint.rotation);
            visualProjectile.Init(projectileParams);
            visualProjectile.Launch();

            if (HasInputAuthority)
                audioSource.Play();

            _visualProjectiles[serverProjectileId] = visualProjectile;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_DestroyDummyProjectile(NetworkId serverProjectileId, Vector3 position, RpcInfo info = default)
        {
            var visualProjectile = _visualProjectiles[serverProjectileId];
            visualProjectile.Explose(position);
            Destroy(visualProjectile.gameObject);
            _visualProjectiles.Remove(serverProjectileId);
        }
    }
}