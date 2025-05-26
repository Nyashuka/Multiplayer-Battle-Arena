using Fusion;
using UnityEngine;

namespace Data
{
    public struct ItemUseContext : INetworkStruct
    {
        public Vector3 ThrowFrom;
        public Vector3 AimDirection;
        public NetworkBehaviour User;
    }
}