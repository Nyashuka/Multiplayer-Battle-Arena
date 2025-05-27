using Core.Projectiles;
using Core.Projectiles.Abstract;
using Core.Projectiles.SmoothedProjectile;
using Fusion;
using UnityEngine;

namespace ScriptableObjects.Weapons.Projectiles
{
    
    [CreateAssetMenu(menuName = "Weapons/Projectiles/Weapon Config")]
    public class ProjectileConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private SmoothedServerProjectile serverProjectilePrefab;
        [SerializeField] private VisualProjectileBase dummyProjectilePrefab;
        
        [Header("Config")]
        [SerializeField] private bool explodesOnHit = false;
        [SerializeField] private float explodeRadius;

        public SmoothedServerProjectile ServerProjectilePrefab => serverProjectilePrefab;
        public VisualProjectileBase DummyProjectilePrefab => dummyProjectilePrefab;
        public bool ExplodesOnHit => explodesOnHit;
        public float ExplodeRadius => explodeRadius;
    }
}