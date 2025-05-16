using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class ServerProjectileWeapon : WeaponBase
    {
        [SerializeField] private DummyProjectile dummyVisualPrefab;
        [SerializeField] private ServerProjectile serverProjectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float speed;

        public override void Fire()
        {
            var projectileParams = new ProjectileParams();
            projectileParams.Direction = firePoint.forward;
            projectileParams.Speed = speed;
            
            if (HasInputAuthority)
            {
                var dummyProjectile = Instantiate(dummyVisualPrefab, firePoint.position, firePoint.rotation);
                dummyProjectile.Init(projectileParams);
                dummyProjectile.Launch();
            }

            if (HasStateAuthority)
            {
                var projectile = Runner.Spawn(serverProjectilePrefab, firePoint.position, firePoint.rotation, Object.InputAuthority);
                projectile.GetComponent<IProjectileInitialize>().Init(projectileParams);
            }
        } 
    }
}