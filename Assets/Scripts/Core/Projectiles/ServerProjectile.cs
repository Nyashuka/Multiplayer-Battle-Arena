using System.Collections.Generic;
using System.Linq;
using Core.PlayerComponents;
using Data;
using Fusion;
using Networking;
using UnityEngine;

namespace Core.Projectiles
{
    public class ServerProjectile : NetworkBehaviour, IProjectileInitialize
    {
        private float speed = 20f;
        public float lifetime = 2f;

        private float _timer;
        private Vector3 _direction;

        public void Init(ProjectileParams projectileParams)
        {
            speed = projectileParams.Speed;
            _direction = _direction = (projectileParams.Target - projectileParams.VisualStart).normalized;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            Vector3 currentPosition = transform.position;
            Vector3 displacement = _direction * (speed * Runner.DeltaTime);
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
                    damagable.TakeDamage(25);
                    Debug.Log("Damaged");
                }
                // if (hit.collider.TryGetComponent<IDamagable>(out var damagable))
                // {
                //     damagable.TakeDamage(25);
                //     Debug.Log("Damaged");
                // }
                Explode(hit.point);
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

        private void Explode(Vector3 position)
        {
            RPCGlobalManger.Instance.Explode(position);
        }
    }
}