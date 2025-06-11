using Fusion;
using UnityEngine;

namespace Utils
{
    public class NetworkLayerSetter : NetworkBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        public override void Spawned()
        {
            if (Object.InputAuthority == Runner.LocalPlayer)
            {
                int layerInt = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
                RecursiveLayerChange(gameObject.transform, layerInt);
            }
        }

        private void RecursiveLayerChange(Transform obj, int layer)
        {
            obj.gameObject.layer = layer;
            foreach (Transform child in obj)
            {
                RecursiveLayerChange(child, layer);
            }
        }
    }
}