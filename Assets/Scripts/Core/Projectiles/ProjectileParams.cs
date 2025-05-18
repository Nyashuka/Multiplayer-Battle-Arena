using Fusion;
using UnityEngine;

namespace Core.Projectiles
{
    public struct ProjectileParams : INetworkStruct
    {
        public float Speed;
        public Vector3 Direction;
        public Vector3 ServerStart;
        public Vector3 VisualStart;
        public Vector3 Target;
        public PlayerRef Owner;
    }
}