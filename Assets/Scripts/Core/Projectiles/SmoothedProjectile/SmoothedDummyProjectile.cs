using Core.Projectiles.Abstract;
using Core.Projectiles.Data;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.Projectiles.SmoothedProjectile
{
    public class SmoothedDummyProjectile : VisualProjectileBase
    {
        private ProjectileParams _projectileParams;
        
        private bool _running = false;
        private float _timer = 0f;
        private Vector3 _direction;
        
        public override void Init(ProjectileParams projectileParams)
        {
            _vfxService = ServiceLocator.Instance.GetService<VFXService>();
            _projectileParams = projectileParams;
            _direction = projectileParams.Direction; //(projectileParams.Target - projectileParams.VisualStart).normalized;
        }

        public override void Launch()
        {
            if(_running) return;

            _running = true;
            _timer = 0f;
        }

        private void Update()
        {
            if(!_running) return;
            
            transform.position += _direction * (_projectileParams.Speed * Time.deltaTime);
            
            _timer += Time.deltaTime;
            if (_timer > _projectileParams.LifeTime)
            {
                _running = false;
                _timer = 0f;
            }
        }
    }
}