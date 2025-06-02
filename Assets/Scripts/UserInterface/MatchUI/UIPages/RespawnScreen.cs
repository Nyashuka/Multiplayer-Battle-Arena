using System.Collections.Generic;
using Fusion;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocator;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    public class RespawnScreen : UIPage
    {
        [SerializeField] private SelectWeaponButton selectWeaponButtonPrefab;
        
        [SerializeField] private RectTransform mainWeaponsParent;
        [SerializeField] private RectTransform utilityItemsParent;
        [SerializeField] private TMP_Text timerText;

        private bool _timeIsExpired = true;
        private float _respawnAt;
        private NetworkRunner _networkRunner;

        private readonly List<SelectWeaponButton> _selectWeaponButtons = new();
        private readonly List<SelectWeaponButton> _selectUtilityItemButtons = new();

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

                if (_selectWeaponButtons.Count == 0)
                {
                    var weapons = ServiceLocator.Instance.GetService<WeaponDatabaseService>().GetAll();
                    foreach (var weapon in weapons)
                    {
                        var weaponButton = Instantiate(selectWeaponButtonPrefab, mainWeaponsParent);
                        _selectWeaponButtons.Add(weaponButton);
                        weaponButton.SetWeapon(weapon.Icon, weapon.ID);
                        weaponButton.OnClick += OnWeaponSelected;
                    }
                }

                if (_selectUtilityItemButtons.Count == 0)
                {
                    var utilities = ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetAll();
                    foreach (var utilityItem in utilities)
                    {
                        var weaponButton = Instantiate(selectWeaponButtonPrefab, utilityItemsParent);
                        _selectUtilityItemButtons.Add(weaponButton);
                        weaponButton.SetWeapon(utilityItem.Icon, utilityItem.Id);
                        weaponButton.OnClick += OnUtilityItemSelected;
                    }
                }
            }
        }

        private void OnUtilityItemSelected(string id)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.UtilityItemRequested, new UtilityItemRequestedEventArgs(id));
            Debug.Log("Raised request utility " + id);
        }

        private void OnWeaponSelected(string id)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.WeaponRequested, new WeaponRequestedEventArgs(id));
            Debug.Log("Raised request weapon " + id);
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