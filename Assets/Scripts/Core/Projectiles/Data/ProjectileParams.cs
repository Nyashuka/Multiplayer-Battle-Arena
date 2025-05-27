using Fusion;
using UnityEngine;

namespace Core.Projectiles.Data
{
    public struct ProjectileParams : INetworkStruct
    {
        public float Speed;
        public Vector3 Direction;
        public Vector3 ServerStart;
        public Vector3 VisualStart;
        public Vector3 Target;
        public float Damage;
        public float LifeTime;
        public PlayerRef Owner;
    }
}