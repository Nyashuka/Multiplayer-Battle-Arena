using Fusion;
using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    
    [CreateAssetMenu(menuName = "Utility Items/Grenade Config")]
    public class GrenadeItemConfig : UtilityItemConfig
    {
        [Header("Prefabs")]
        [SerializeField] private NetworkPrefabRef grenadePrefab;
        
        [Header("Config")]
        [SerializeField] private int damage;
        [SerializeField] private float range;
        [SerializeField] private float throwForce;
        [SerializeField] private float explodeDelay;
        
        public NetworkPrefabRef GrenadePrefab => grenadePrefab;
        public int Damage => damage;
        public float Range => range;
        public float ThrowForce => throwForce;
        public float ExplodeDelay => explodeDelay;
    }
}