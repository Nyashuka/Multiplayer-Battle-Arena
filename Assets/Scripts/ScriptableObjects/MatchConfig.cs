using Fusion;
using UnityEngine;
using UserInterface;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/MatchConfig")]
    public class MatchConfig : ScriptableObject
    {
        [Header("Network Prefabs")]
        public NetworkPrefabRef mapPrefab;
        public NetworkPrefabRef playerPrefab;
        public NetworkPrefabRef matchManagerPrefab;

        [Header("UI Prefabs")]
        public Canvas canvasPrefab;
        public HUDManager hudPrefab;
    }
}