using Core.PlayerComponents;
using UnityEngine;

namespace Core.EnemyComponents
{
    public class EnemyCanvas : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransformForFollow;
        [SerializeField] private EnemyHealthBar healthBar;

        public void Initialize(Transform cameraTransform, IHealthSource health)
        {
            cameraTransformForFollow = cameraTransform;
            InitializeHealthBar(health);
        }

        private void InitializeHealthBar(IHealthSource health)
        {
            healthBar.Initialize(health);
        }
        
        private void LateUpdate()
        {
            transform.LookAt(transform.position + cameraTransformForFollow.forward);    
        }
    }
}
