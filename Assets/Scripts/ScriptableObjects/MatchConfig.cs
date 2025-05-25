using Fusion;
using UnityEngine;
using UserInterface;
using UserInterface.MatchUI;

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
        [SerializeField] private NetworkPrefabRef matchTimerPrefab;

        public NetworkPrefabRef DefaultWeaponPrefab => defaultWeaponPrefab;
        public NetworkPrefabRef MatchTimerPrefab => matchTimerPrefab;
        
        [Header("UI Prefabs")]
        public Canvas canvasPrefab;
        public GameHUD gameHUDPrefab;
        [SerializeField] private MatchTimerUI matchTimerUIPrefab;
        [SerializeField] private MatchUIController matchUIControllerPrefab;
        
        public MatchTimerUI MatchTimerUIPrefab => matchTimerUIPrefab;
        public MatchUIController MatchUIControllerPrefab => matchUIControllerPrefab;
    }
}