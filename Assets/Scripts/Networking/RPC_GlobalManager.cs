using Fusion;
using UnityEngine;
using VFX;

namespace Networking
{
    public class RPCGlobalManger : NetworkBehaviour
    {
        public static RPCGlobalManger Instance { get; private set; }
        
        [SerializeField] private SimpleExplosiveEffect explosionPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Duplicate instance of RPC_GlobalManager!");
                Destroy(gameObject);
            }
        }
        
        public void Explode(Vector3 position)
        {
            RPC_SpawnExplosion(position);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnExplosion(Vector3 position)
        {
            var explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.Play();
        }
    }
}
