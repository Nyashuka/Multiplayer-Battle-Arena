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
            Vector3 hitPoint = Physics.Raycast(ray, out var hit, 100f, ~LayerMask.GetMask("Projectile"))
                ? hit.point
                : ray.GetPoint(100f);

            gunTarget.rotation = Quaternion.LookRotation((hitPoint - gunTarget.position).normalized);
        }
        
        public void GetAim(out Vector3 origin, out Vector3 direction)
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            origin = ray.origin;
            direction = ray.direction;
        }
    }
}