using Data;
using UnityEngine;

namespace Core.Projectiles
{
    public class DummyProjectile : MonoBehaviour, IProjectileInitialize
    {
        private float _speed = 20f;
        public float lifetime = 2f;

        private float _timer = 0f;
        private Vector3 _direction;
        
        public void Init(ProjectileParams data)
        {
            _speed = data.Speed;
            _direction = data.Direction.normalized;
        }

        public void Launch()
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