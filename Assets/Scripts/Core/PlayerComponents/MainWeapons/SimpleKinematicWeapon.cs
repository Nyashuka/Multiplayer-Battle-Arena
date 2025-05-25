using System.Collections.Generic;
using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using Core.Projectiles.Abstract;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class SimpleKinematicWeapon : WeaponBase
    {
        [SerializeField] private VisualProjectileBase visualProjectilePrefab;
        [SerializeField] private ServerProjectile serverProjectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float speed;
        
        [SerializeField] private AudioSource audioSource;

        private Dictionary<NetworkId, VisualProjectileBase> _visualProjectiles = new();
        
        public override void Fire(Vector3 start, Vector3 direction)
        {
            if (!HasInputAuthority) return;

            var projectileParams = new ProjectileParams
            {
                VisualStart = firePoint.position,
                ServerStart = start,
                Direction = direction,
                Speed = speed,
                Owner = Object.InputAuthority
            };

            RPC_RequestFire(projectileParams);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_RequestFire(ProjectileParams projectileParams, RpcInfo info = default)
        {
            if (!HasStateAuthority) return;
            
            projectileParams.Speed = speed;
            
            if (Physics.Raycast(projectileParams.ServerStart, projectileParams.Direction, out RaycastHit hit, 100f))
            {
                projectileParams.Target = hit.point;
            }
            else
            {
                projectileParams.Target = projectileParams.ServerStart + projectileParams.Direction * 100f;
            }
            
            var serverProjectile = Runner.Spawn(serverProjectilePrefab, firePoint.position, firePoint.rotation, Object.InputAuthority);
            serverProjectile.weapon = this;
            serverProjectile.GetComponent<IProjectileInitialize>().Init(projectileParams);

            RPC_SpawnDummyProjectile(serverProjectile.Object.Id, projectileParams);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnDummyProjectile(NetworkId serverProjectileId, ProjectileParams projectileParams, RpcInfo info = default)
        {
            var visualProjectile = Instantiate(visualProjectilePrefab, firePoint.position, firePoint.rotation);
            visualProjectile.Init(projectileParams);
            visualProjectile.Launch();
            
            if(HasInputAuthority)
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