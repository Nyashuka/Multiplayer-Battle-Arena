using Fusion;
using UnityEngine;

namespace Data
{
    public struct GameplayInput : INetworkInput
    {
        public Vector2        MoveDirection;
        public Vector2        LookRotationDelta;
        public NetworkButtons Actions;

        public const int JUMP_BUTTON = 0;
        public const int FIRE_BUTTON = 1;
        public const int USE_UTILITY_BUTTON = 2;
    }
}