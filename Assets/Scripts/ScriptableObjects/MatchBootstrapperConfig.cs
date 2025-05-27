using Fusion;
using ScriptableObjects.Weapons;
using UnityEngine;
using UserInterface;
using UserInterface.MatchUI;
using UserInterface.MatchUI.HUDElements;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/MatchConfig")]
    public class MatchBootstrapperConfig : ScriptableObject
    {
        [Header("Network Prefabs")]
        [SerializeField] public NetworkPrefabRef mapPrefab;
        [SerializeField] public NetworkPrefabRef playerPrefab;
        [SerializeField] public NetworkPrefabRef matchManagerPrefab;
        [SerializeField] private WeaponConfigBase defaultWeaponConfig;
        [SerializeField] private NetworkPrefabRef matchTimerPrefab;

        public WeaponConfigBase DefaultWeaponConfig => defaultWeaponConfig;
        public NetworkPrefabRef MatchTimerPrefab => matchTimerPrefab;
        
        [Header("UI Prefabs")]
        [SerializeField] private GameHUD gameHUDPrefab;
        [SerializeField] private UIPageSwitcher uiPageSwitcherPrefab;
        [SerializeField] private MatchTimerUI matchTimerUIPrefab;
        
        public GameHUD GameHUDPrefab => gameHUDPrefab;
        public MatchTimerUI MatchTimerUIPrefab => matchTimerUIPrefab;
        public UIPageSwitcher UIPageSwitcherPrefab => uiPageSwitcherPrefab;
    }
    
}