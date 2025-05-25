using Core.MatchmakingComponents;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace UserInterface.MatchUI
{
    public class MatchUIController : MonoBehaviour
    {
        [SerializeField] private GameHUD gameHUD;
        [SerializeField] private RespawnScreen respawnScreen;
        [SerializeField] private MatchTimerUI matchTimerUI;

        private void Start()
        {
            respawnScreen.gameObject.SetActive(false);
            matchTimerUI.Initialize(MatchManager.Instance.MatchTimer);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.ShowRespawnScreen, OnShowRespawnScreen);
        }

        private void OnShowRespawnScreen(IEventBusArgs args)
        {
            if (args is RespawnEventArgs respawnEventArgs)
            {
                gameHUD.gameObject.SetActive(false);
                matchTimerUI.gameObject.SetActive(false);
                respawnScreen.gameObject.SetActive(true);

                respawnScreen.SetRespawnTime(respawnEventArgs.RespawnAt);

                respawnScreen.CanRespawn += OnCanRespawn;
            }
        }

        private void OnCanRespawn()
        {
            gameHUD.gameObject.SetActive(true);
            matchTimerUI.gameObject.SetActive(true);
            respawnScreen.gameObject.SetActive(false);
            
            respawnScreen.CanRespawn -= OnCanRespawn;
        }
    }
}