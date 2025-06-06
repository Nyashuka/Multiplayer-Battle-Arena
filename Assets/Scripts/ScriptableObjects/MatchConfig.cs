using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/MatchConfig")]
    public class MatchConfig : ScriptableObject
    {
        [Tooltip("Warmup time in seconds")] [SerializeField] 
        private float warmupDuration = 10f;

        [Tooltip("Match time in seconds")] [SerializeField]
        private float matchDuration = 240f;
        
        public float WarmupDuration => warmupDuration;
        public float MatchDuration => matchDuration;

    }
}