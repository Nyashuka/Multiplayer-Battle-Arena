using Fusion;
using UnityEngine;

namespace Data
{
    public struct UtilityItemUseContext : INetworkStruct
    {
        public Vector3 ThrowFrom;
        public Vector3 AimDirection;
        public PlayerRef Owner;
    }
}