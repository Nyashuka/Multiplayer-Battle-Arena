using System;
using Fusion;
using UnityEngine;

namespace Core.Projectiles.Data
{
    public struct ProjectileParams : INetworkStruct
    {
        public Guid Id;
        public float Speed;
        public Vector3 Direction;
        public Vector3 CameraStart;
        public Vector3 VisualStart;
        public Vector3 Target;
        public int Damage;
        public float LifeTime;
        public PlayerRef Owner;
    }
}