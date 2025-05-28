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
        [SerializeField] private NetworkPrefabRef weaponDealerPrefab;
        [SerializeField] private WeaponConfigBase defaultWeaponConfig;
        [SerializeField] private NetworkPrefabRef matchTimerPrefab;
        [SerializeField] private WeaponList weaponList;

        public WeaponConfigBase DefaultWeaponConfig => defaultWeaponConfig;
        public NetworkPrefabRef MatchTimerPrefab => matchTimerPrefab;
        
        [Header("UI Prefabs")]
        [SerializeField] private GameHUD gameHUDPrefab;
        public GameHUD GameHUDPrefab => gameHUDPrefab;
        public WeaponList WeaponList => weaponList;
        public NetworkPrefabRef WeaponDealerPrefab => weaponDealerPrefab;
    }
    
}