using System;
using Core.MatchmakingComponents.MatchStates;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using UserInterface.MatchUI.UIPages;

namespace UserInterface.MatchUI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIPageSwitcher pageSwitcher;
        
        private GameHUD _gameHud;
        public GameHUD GameHUD => _gameHud;
        
        public static UIManager Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            OpenInitialPage();
        }

        public void OpenInitialPage()
        {
            pageSwitcher.SwitchPage<MainMenu>();
        }

        public void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStateChanged, OnMatchStateChanged);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerRespawnStarted, OnStartRespawn);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerRespawned, OnPlayerRespawned);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.LeaderboardDataAvailable, OnShowLeaderboard);
        }

        private void OnShowLeaderboard(IEventBusArgs e)
        {
            Debug.Log("LeaderBoard event");
            if (e is ShowLeaderboardEventArgs leaderboardEventArgs)
            {
                Debug.Log("Leaderboard");
                _gameHud.HideAll();
                var page = pageSwitcher.SwitchPage<ScoreBoard>();
                page.Initialize(leaderboardEventArgs.LeaderboardData);
            }
            
        }

        private void OnPlayerRespawned(IEventBusArgs e)
        {
            pageSwitcher.ClosePage();
            _gameHud.ShowAll();
        }

        private void OnStartRespawn(IEventBusArgs e)
        {
            _gameHud.HideAll();
            pageSwitcher.SwitchPage<RespawnScreen>();
        }

        private void OnMatchStateChanged(IEventBusArgs e)
        {
            if (e is MatchStateChangedEventArgs matchStateChangedEventArgs)
            {
                if (matchStateChangedEventArgs.State != MatchStateEnum.Ending)
                {
                    pageSwitcher.ClosePage();
                }
            }
        }

        public void SetHud(GameHUD gameHud)
        {
            _gameHud = gameHud;
            _gameHud.ShowAll();
        }
    }
}