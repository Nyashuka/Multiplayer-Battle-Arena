using Core.EnemyComponents;
using Core.PlayerComponents.HealthComponent;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using Services.Audio;
using Services.ServiceLocatorModule;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class PlayerLifecycle : NetworkBehaviour
    {
        [SerializeField] private SimpleKCC kcc;
        [SerializeField] private GameObject gunRoot;
        [SerializeField] private GameObject playerVisualRoot;
        [SerializeField] private EnemyCanvasHandler enemyCanvasHandler;
        [SerializeField] private NetworkHealth networkHealth;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip respawnedSound;
        
        public bool IsEnabled { get; private set;  } = true;

        public void Die()
        {
            IsEnabled = false;
            
            Rpc_DeathPlayer();
        }

        public void Respawn(Transform respawnPosition)
        {
            IsEnabled = true;
            
            kcc.SetPosition(respawnPosition.position);
            kcc.SetLookRotation(respawnPosition.rotation);
            networkHealth.ResetHealth();
            
            Rpc_RespawnPlayer();
        }

        private void ShowLocalVisual()
        {
            kcc.Rigidbody.isKinematic = false;
            gunRoot.SetActive(true);
            playerVisualRoot.SetActive(true);
            enemyCanvasHandler.Enable();
        }

        private void HideLocalVisual()
        {
            gunRoot.SetActive(false);
            playerVisualRoot.SetActive(false);
            kcc.Rigidbody.isKinematic = true;
            enemyCanvasHandler.Disable();
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_DeathPlayer()
        {
            HideLocalVisual();
            if (HasInputAuthority)
            {
                ServiceLocator.Instance.GetService<AudioService>()
                    .PlaySfx(deathSound, transform.position);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_RespawnPlayer()
        {
            ShowLocalVisual();
            if (HasInputAuthority)
            {
                ServiceLocator.Instance.GetService<AudioService>()
                    .PlaySfx(respawnedSound, transform.position);
            }
        } 
    }
}