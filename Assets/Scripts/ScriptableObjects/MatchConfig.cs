using Fusion;
using UnityEngine;
using UserInterface;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/MatchConfig")]
    public class MatchConfig : ScriptableObject
    {
        [Header("Network Prefabs")]
        [SerializeField] public NetworkPrefabRef mapPrefab;
        [SerializeField] public NetworkPrefabRef playerPrefab;
        [SerializeField] public NetworkPrefabRef matchManagerPrefab;
        [SerializeField] private NetworkPrefabRef defaultWeaponPrefab;

        public NetworkPrefabRef DefaultWeaponPrefab => defaultWeaponPrefab;
        
        [Header("UI Prefabs")]
        public Canvas canvasPrefab;
        public HUDManager hudPrefab;
    }
}