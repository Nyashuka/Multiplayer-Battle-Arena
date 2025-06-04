using Core.Projectiles.Data;
using Services.VFXs;
using UnityEngine;
using VFX;

namespace Core.Projectiles.Abstract
{
    public abstract class VisualProjectileBase : MonoBehaviour, IProjectileInitialize
    {
        [SerializeField] protected ParticleSystem burstEffectPrefab;
        protected VFXService _vfxService;
        
        public abstract void Init(ProjectileParams projectileParams);
        public abstract void Launch();
        
        public void Explode(Vector3 position)
        {
            _vfxService.PlayLocalVFX(burstEffectPrefab, position, Quaternion.identity);
        }
    }
}