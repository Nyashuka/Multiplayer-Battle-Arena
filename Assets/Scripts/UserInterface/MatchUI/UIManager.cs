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
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStarted, OnMatchStarted);
        }

        private void OnMatchStarted(IEventBusArgs e)
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