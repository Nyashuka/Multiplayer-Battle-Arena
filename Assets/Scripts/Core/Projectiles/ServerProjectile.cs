using System.Collections.Generic;
using System.Linq;
using Data;
using Fusion;
using Networking;
using UnityEngine;
using VFX;

namespace Core.Projectiles
{
    public class ServerProjectile : NetworkBehaviour, IProjectileInitialize
    {
        private float speed = 20f;
        public float lifetime = 2f;

        private float timer;
        private Vector3 direction;


        public void Init(ProjectileParams data)
        {
            speed = data.Speed;
            direction = data.Direction;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            Vector3 currentPosition = transform.position;
            Vector3 displacement = direction * (speed * Runner.DeltaTime);
            Vector3 nextPosition = currentPosition + displacement;

            if (Runner.GetPhysicsScene().Raycast(currentPosition, direction, out var hit, displacement.magnitude))
            {
                Explode(hit.point);
                Runner.Despawn(Object);
                return;
            }

            transform.position = nextPosition;

            timer += Runner.DeltaTime;
            if (timer > lifetime)
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