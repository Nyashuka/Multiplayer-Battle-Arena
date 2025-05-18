using Data;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Game/MovementSettings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [SerializeField] private MovementConfig config;

        public MovementConfig GetConfig()
        {
            return config.Equals(default) ? MovementConfig.GetDefault() : config;        }
    }
}