using Core.PlayerComponents;
using Core.PlayerComponents.HealthComponent;
using Fusion;
using TMPro;
using UnityEngine;

namespace Core.EnemyComponents
{
    public class EnemyCanvas : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransformForFollow;
        [SerializeField] private EnemyHealthBar healthBar;
        [SerializeField] private TMP_Text playerNameText;

        public void Initialize(Transform cameraTransform, IHealthSource health, PlayerRef playerRef)
        {
            cameraTransformForFollow = cameraTransform;
            playerNameText.text = playerRef.ToString();
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
