using Fusion;
using UnityEngine;

namespace Core
{
    public struct  NetworkInputData : INetworkInput
    {
        public const byte MOUSEBUTTON0 = 1;
        public const byte MOUSEBUTTON1 = 2;
        
        public NetworkButtons buttons;
        
        public Vector3 moveDirection;
        public Vector3 lookDirection;
        public Vector2 lookRotationDelta;
        public float mouseX;
        public float mouseY;
    }
}