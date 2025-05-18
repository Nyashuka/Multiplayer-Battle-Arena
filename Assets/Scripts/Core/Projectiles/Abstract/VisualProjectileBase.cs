using UnityEngine;

namespace Core.Projectiles.Abstract
{
    public abstract class VisualProjectileBase : MonoBehaviour, IProjectileInitialize
    {
        public abstract void Init(ProjectileParams projectileParams);
        public abstract void Launch();
    }
}