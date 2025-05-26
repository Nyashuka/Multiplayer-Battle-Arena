using System;
using Fusion.Addons.SimpleKCC;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class PlayerLifecycle : MonoBehaviour
    {
        [SerializeField] private SimpleKCC simpleKcc;
        [SerializeField] private GameObject gunRoot;
        [SerializeField] private GameObject playerVisualRoot;
        [SerializeField] private EnemyCanvasHandler enemyCanvasHandler;
        [SerializeField] private NetworkHealth networkHealth;
        
        public bool IsEnabled { get; private set;  } = true;

        public void Die()
        {
            IsEnabled = false;
            
            gunRoot.SetActive(false);
            playerVisualRoot.SetActive(false);
            simpleKcc.Collider.gameObject.SetActive(false);
            enemyCanvasHandler.Disable();
        }

        public void Respawn()
        {
            IsEnabled = true;
            
            gunRoot.SetActive(true);
            playerVisualRoot.SetActive(true);
            simpleKcc.Collider.gameObject.SetActive(true);
            enemyCanvasHandler.Enable();
        }
    }
}