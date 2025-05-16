using Fusion;
using UnityEngine;

namespace Core.Projectiles
{
    public struct ProjectileParams : INetworkStruct
    {
        public float Speed;
        public Vector3 Direction;
        public PlayerRef Owner;
    }
}