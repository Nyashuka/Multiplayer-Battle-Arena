using Core.MainWeapons;
using Core.PlayerComponents;
using Core.PlayerComponents.HealthComponent;
using Core.Projectiles.Data;
using Data;
using Fusion;
using ScriptableObjects.Weapons.Projectiles;
using UnityEngine;

namespace Core.Projectiles.SmoothedProjectile
{
    public class SmoothedServerProjectile : NetworkBehaviour, IProjectileInitialize
    {
        public SimpleKinematicWeapon weapon;
        private ProjectileParams _projectileParams;

        private float _timer;
        
        public void Init(ProjectileParams projectileParams)
        {
            _projectileParams = projectileParams;
            _projectileParams.Direction = (projectileParams.Target - projectileParams.VisualStart).normalized;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || weapon == null) return;

            Vector3 currentPosition = transform.position;
            Vector3 displacement = _projectileParams.Direction * (_projectileParams.Speed * Runner.DeltaTime);
            Vector3 nextPosition = currentPosition + displacement;
            
            if (Runner.GetPhysicsScene().Raycast(currentPosition, _projectileParams.Direction, out var hit, displacement.magnitude))
            {
                IDamagable damagable = null;
                
                if (hit.collider.TryGetComponent<IDamagable>(out var directHit))
                {
                    damagable = directHit;
                }
                else if (hit.collider.transform.root.TryGetComponent<IDamagable>(out var rootHit))
                {
                    damagable = rootHit;
                }

                if (damagable != null && damagable.Owner != _projectileParams.Owner)
                {
                    damagable.TakeDamage(new DamageData()
                    {
                        Attacker = _projectileParams.Owner,
                        Damage = _projectileParams.Damage
                    });
                    Debug.Log("Damaged");
                }

                if (damagable == null || damagable.Owner != _projectileParams.Owner)
                {
                    weapon.RPC_DestroyDummyProjectile(_projectileParams.Id, hit.point);
                    Runner.Despawn(Object);
                    return;
                }
            }

            transform.position = nextPosition;

            _timer += Runner.DeltaTime;
            if (_timer > _projectileParams.LifeTime)
            {
                weapon.RPC_DestroyDummyProjectile(_projectileParams.Id, hit.point);
                Runner.Despawn(Object);
            }
        }
    }
}