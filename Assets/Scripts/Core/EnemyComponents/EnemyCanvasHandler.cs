using Core.PlayerComponents.HealthComponent;
using Fusion;
using UnityEngine;

namespace Core.EnemyComponents
{
    public class EnemyCanvasHandler : NetworkBehaviour
    {
        [SerializeField] private NetworkHealth networkHealth;
        [SerializeField] private EnemyCanvas enemyCanvasPrefab;
        
        private EnemyCanvas _canvasInstance;

        public override void Spawned()
        {
            if (!Object.HasInputAuthority)
            {
                _canvasInstance = Instantiate(enemyCanvasPrefab, transform, false);
                _canvasInstance.Initialize(Camera.main.transform, networkHealth, Object.InputAuthority);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}