using System;
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
                pageSwitcher.SwitchPage<MainMenu>();
            }
        }

        public void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStateChanged, OnMatchStateChanged);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StartRespawn, OnStartRespawn);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerRespawned, OnPlayerRespawned);
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
            pageSwitcher.ClosePage();
        }

        public void SetHud(GameHUD gameHud)
        {
            _gameHud = gameHud;
            _gameHud.ShowAll();
        }
    }
}