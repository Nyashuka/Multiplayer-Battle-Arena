using Fusion;
using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    
    [CreateAssetMenu(menuName = "Utility Items/Shield Config")]
    public class ShieldItemConfig : UtilityItemConfig
    {
        [Header("Config")]
        [Tooltip("Damage reduction in percentage (for example 0.5 = 50%)")] 
        [Range(0f, 1f)]
        [SerializeField] private float damageReductionMultiplier = 0.5f;

        [Tooltip("Duration in seconds")]
        [SerializeField] private float duration = 5;
        
        [Header("Prefabs")]
        [SerializeField] private NetworkPrefabRef shieldPrefab;
        
        [Header("Effects")]
        [SerializeField] private ParticleSystem shieldEffect;
        
        public float DamageReductionMultiplier => damageReductionMultiplier;
        public float Duration => duration;
        public NetworkPrefabRef ShieldPrefab => shieldPrefab;
        public ParticleSystem ShieldEffect => shieldEffect;
    }
}