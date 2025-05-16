using System.Collections.Generic;
using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class SimpleKinematicWeapon : WeaponBase
    {
        [SerializeField] private DummyProjectile dummyVisualPrefab;
        [SerializeField] private ServerProjectile serverProjectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float speed;

        private Dictionary<NetworkId, DummyProjectile> dummyProjectiles = new();
        
        public override void Fire()
        {
            if (!HasInputAuthority) return;

            var projectileParams = new ProjectileParams();
            projectileParams.Direction = firePoint.forward;
            projectileParams.Speed = speed;

            RPC_RequestFire(projectileParams);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_RequestFire(ProjectileParams projectileParams, RpcInfo info = default)
        {
            if (!HasStateAuthority) return;

            var serverProjectile = Runner.Spawn(serverProjectilePrefab, firePoint.position, firePoint.rotation, Object.InputAuthority);
            serverProjectile.GetComponent<IProjectileInitialize>().Init(projectileParams);

            RPC_SpawnDummyProjectile(serverProjectile.Object.Id, projectileParams);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnDummyProjectile(NetworkId serverProjectileId, ProjectileParams projectileParams, RpcInfo info = default)
        {
            var dummyProjectile = Instantiate(dummyVisualPrefab, firePoint.position, firePoint.rotation);
            dummyProjectile.Init(projectileParams);
            dummyProjectile.Launch();

            dummyProjectiles[serverProjectileId] = dummyProjectile;
        }
    }
}