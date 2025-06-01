using Fusion.Addons.SimpleKCC;
using UnityEngine;

namespace Core.PlayerComponents
{
    public sealed class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraHandle;
        [SerializeField] private Transform gunTarget;
        [SerializeField] private SimpleKCC kcc;

        private Camera _camera;
        private Transform _mainCam;
        private PlayerInput _input;
        [SerializeField] private float minDistance = 2f;
        private float _maxDistance = 100f;

        public void Init(PlayerInput input)
        {
            _input = input;
            _camera = Camera.main;
            if (_camera != null) _mainCam = _camera.transform;
        }

        public void Tick()
        {
            Vector2 look = kcc.GetLookRotation(true, false);
            cameraHandle.localRotation = Quaternion.Euler(look);
            _mainCam.SetPositionAndRotation(cameraHandle.position, cameraHandle.rotation);

            AimGun();
        }

        private void AimGun()
        {
            Ray ray = new Ray(_mainCam.position, _mainCam.forward);
            Vector3 hitPoint = ray.GetPoint(_maxDistance);
            
            if (Physics.Raycast(ray, out var hit, _maxDistance, ~LayerMask.GetMask("Projectile")))
            {
                if (hit.distance < minDistance) return;
                
                hitPoint = hit.point;
            }

            gunTarget.rotation = Quaternion.LookRotation((hitPoint - gunTarget.position).normalized);
        }
        
        public void GetAim(out Vector3 origin, out Vector3 direction)
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    
            int mask = ~LayerMask.GetMask("Player");
    
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
            {
                origin = ray.origin;
                direction = (hit.point - origin).normalized;
            }
            else
            {
                origin = ray.origin;
                direction = ray.direction;
            }
            
            Debug.DrawRay(ray.origin, ray.direction * _maxDistance, Color.red, 2f);
        }
    }
}