using Fusion;
using UnityEngine;

namespace Data
{
    public struct ProjectileData : INetworkStruct
    {
        public Vector3 StartPosition; 
        public Vector3 Direction;    
    }
}