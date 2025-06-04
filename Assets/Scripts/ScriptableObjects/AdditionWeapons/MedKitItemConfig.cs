using Fusion;
using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    [CreateAssetMenu(menuName = "Utility Items/Medkit Config")]
    public class MedKitItemConfig : UtilityItemConfig
    {   
        [Header("Config")]
        [SerializeField] private int healAmount;
        
        [Header("Prefabs")]
        [SerializeField] private NetworkPrefabRef medKitPrefab;
        
        [Header("Effects")]
        [SerializeField] private ParticleSystem medKitEffect;
        
        [Header("Sound")]
        [SerializeField] private AudioClip medKitSound;
        
        public int HealAmount => healAmount;
        public NetworkPrefabRef MedKitPrefab => medKitPrefab;
        public ParticleSystem MedKitEffect => medKitEffect;
        public AudioClip MedKitSound => medKitSound;
    }
}