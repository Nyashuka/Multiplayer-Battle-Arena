using UnityEngine;
using VFX;

namespace Core.Projectiles.Abstract
{
    public abstract class VisualProjectileBase : MonoBehaviour, IProjectileInitialize
    {
        [SerializeField] protected SimpleExplosiveEffect explosiveEffect;
        public abstract void Init(ProjectileParams projectileParams);
        public abstract void Launch();

        public void Explose(Vector3 position)
        {
            var explosion = Instantiate(explosiveEffect, position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, 1f);
        }
    }
}