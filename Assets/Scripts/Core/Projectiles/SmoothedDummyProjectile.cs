using Core.Projectiles.Abstract;
using UnityEngine;

namespace Core.Projectiles
{
    public class SmoothedDummyProjectile : VisualProjectileBase
    {
        private float _speed = 20f;
        public float lifetime = 2f;

        private float _timer = 0f;
        private Vector3 _direction;
        
        public override void Init(ProjectileParams projectileParams)
        {
            _speed = projectileParams.Speed;
            _direction = (projectileParams.Target - projectileParams.VisualStart).normalized;
        }

        public override void Launch()
        {
            _timer = 0f;
        }

        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
            
            _timer += Time.deltaTime;
            if (_timer > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}