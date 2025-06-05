using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;
using UserInterface.MatchUI.Components;

namespace UserInterface.MatchUI.UIPages
{
    public class RespawnScreen : UIPage
    {
        [SerializeField] private MainWeaponSelectionUIComponent weaponSelectionComponent;
        [SerializeField] private UtilityItemSelectionUIComponent utilityItemSelectionComponent;
        [SerializeField] private TMP_Text timerText;

        private bool _timeIsExpired = true;
        private float _respawnAt;
        private NetworkRunner _networkRunner;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerRespawnStarted, OnStartRespawn);
        }
        
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerRespawnStarted, OnStartRespawn);
        }

        private void OnStartRespawn(IEventBusArgs args)
        {
            if (args is StartRespawnEventArgs startRespawnEventArgs)
            {
                _networkRunner = startRespawnEventArgs.Runner;
                SetRespawnTime(startRespawnEventArgs.RespawnAt);

                weaponSelectionComponent.Initialize();
                utilityItemSelectionComponent.Initialize(); 
            }
        }

        private void OnUtilityItemSelected(string id)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.UtilityItemRequested, new UtilityItemRequestedEventArgs(id));
            Debug.Log("Raised request utility " + id);
        }

        private void SetRespawnTime(float respawnAt)
        {
            _respawnAt = respawnAt;
            _timeIsExpired = false;
        }

        public void Update()
        {
            if(_timeIsExpired) return;
            
            var timeLeft = _respawnAt - _networkRunner.SimulationTime;

            if (timeLeft <= 0)
            {
                _timeIsExpired = true;
                timerText.text = "00";
                return;
            }

            int secondsLeft = Mathf.CeilToInt(timeLeft);
            timerText.text = secondsLeft.ToString("D2");
        }
    }
}