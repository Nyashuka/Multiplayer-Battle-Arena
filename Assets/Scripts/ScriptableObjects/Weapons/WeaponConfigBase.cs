using System.Net.NetworkInformation;
using Fusion;
using ScriptableObjects.Weapons.Projectiles;
using UnityEngine;

namespace ScriptableObjects.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Weapon Config")]
    public class WeaponConfigBase : ScriptableObject
    {
        [Header("Item Config")]
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        
        [Header("Weapon Config")]
        [SerializeField] private float damage;
        [SerializeField] private float fireRate;
        [SerializeField] private float muzzleVelocity;
        [SerializeField] private float maxDistance = 100f;
        [SerializeField] private float bulletLifeTime = 2.5f;
        [SerializeField] private NetworkPrefabRef weaponPrefabRef;
        
        [Header("Projectile Config")]
        [SerializeField] private ProjectileConfig projectileConfig;
        
        public string ID => id;
        public Sprite Icon => icon;
        public float Damage => damage;
        public float FireRate => fireRate;
        public float MuzzleVelocity => muzzleVelocity;
        public float MaxDistance => maxDistance;
        public float BulletLifeTime => bulletLifeTime;
        public ProjectileConfig ProjectileConfig => projectileConfig;
        public NetworkPrefabRef WeaponPrefabRef => weaponPrefabRef;
    }
}