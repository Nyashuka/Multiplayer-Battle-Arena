using Data;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Game/MovementSettings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [SerializeField] private MovementConfig config;
        [Range(0, 5)]
        [SerializeField] private float dashDistance = 5;
        [Range(0, 5)]
        [SerializeField] private float dashCooldown = 2;
        [SerializeField] private float doubleTapThreshold = 0.1f;

        public float DashDistance => dashDistance;
        public float DashCooldown => dashCooldown;
        public float DoubleTapThreshold => doubleTapThreshold;
        
        public MovementConfig GetConfig()
        {
            return config.Equals(default) ? MovementConfig.GetDefault() : config;        
        }
    }
}