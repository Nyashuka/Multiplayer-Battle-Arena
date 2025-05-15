using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.UtilityItems.Abstract
{
    public abstract class UtilityItem : MonoBehaviour
    {
        public abstract void Activate(Vector3 direction, NetworkRunner runner, PlayerRef owner);
    }
}