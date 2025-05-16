using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class SimpleWeapon : Weapon
    {
        [SerializeField] private ProjectilesLauncher projectilesLauncher;
        [SerializeField] private ServerProjectile bulletPrefab; 
        [Networked] private TickTimer delay { get; set; } 
        
        public override void Fire(Vector3 direction, NetworkRunner runner, PlayerRef owner)
        {
            if (!HasStateAuthority)
                return;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                delay = TickTimer.CreateFromSeconds(runner, 0.5f);

                var projectileParams = new ProjectileParams();
                projectileParams.Direction = direction;
                projectileParams.Speed = 10f;
                projectileParams.Owner = owner;

                Runner.Spawn(
                    bulletPrefab,
                    transform.position + transform.forward, // правильна позиція спавну
                    Quaternion.identity,
                    owner,
                    (runner, o) =>
                    {
                        o.GetComponent<IProjectileInitialize>().Init(projectileParams);
                        // o.GetComponent<PhysxBall>().Init(transform.forward);
                    });
            }
        }
    }
}