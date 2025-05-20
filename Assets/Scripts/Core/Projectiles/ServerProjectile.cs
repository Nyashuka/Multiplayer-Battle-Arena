using Core.PlayerComponents;
using Core.PlayerComponents.MainWeapons;
using Data;
using Fusion;
using Networking;
using UnityEngine;

namespace Core.Projectiles
{
    public class ServerProjectile : NetworkBehaviour, IProjectileInitialize
    {
        public SimpleKinematicWeapon weapon;
        public PlayerRef Owner { get; private set; }
        private float _speed = 20f;
        public float lifetime = 2f;

        private float _timer;
        private Vector3 _direction;

        public void Init(ProjectileParams projectileParams)
        {
            _speed = projectileParams.Speed;
            _direction = _direction = (projectileParams.Target - projectileParams.VisualStart).normalized;
            Owner = projectileParams.Owner;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || weapon == null) return;

            Vector3 currentPosition = transform.position;
            Vector3 displacement = _direction * (_speed * Runner.DeltaTime);
            Vector3 nextPosition = currentPosition + displacement;

            if (Runner.GetPhysicsScene().Raycast(currentPosition, _direction, out var hit, displacement.magnitude))
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

                if (damagable != null)
                {
                    damagable.TakeDamage(new DamageData()
                    {
                        Attacker = Owner,
                        Damage = 25
                    });
                    Debug.Log("Damaged");
                }
                // Explode(hit.point);
                
                weapon.RPC_DestroyDummyProjectile(Object.Id, hit.point);
                Runner.Despawn(Object);
                return;
            }

            transform.position = nextPosition;

            _timer += Runner.DeltaTime;
            if (_timer > lifetime)
            {
                Runner.Despawn(Object);
            }
        }
    }
}