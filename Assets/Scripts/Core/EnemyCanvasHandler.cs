using System.ComponentModel;
using Core.PlayerComponents;
using Fusion;
using UnityEngine;

namespace Core
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
                _canvasInstance.Initialize(Camera.main.transform, networkHealth);
            }
        }

        public void Disable()
        {
            
        }

        public void Enable()
        {
            
        }
    }
}