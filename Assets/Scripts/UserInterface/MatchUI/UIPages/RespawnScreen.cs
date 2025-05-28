using System;
using System.Collections.Generic;
using Core.MatchmakingComponents;
using Fusion;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
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

        private List<SelectWeaponButton> _selectWeaponButtons = new();

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStarted, OnMatchStarted);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StartRespawn, OnStartRespawn);
        }
        
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.MatchStarted, OnMatchStarted);
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.StartRespawn, OnStartRespawn);
        }

        private void OnMatchStarted(IEventBusArgs e)
        {
            if (e is MatchStartedEventArgs matchStartedEventArgs)
            {
                _networkRunner = matchStartedEventArgs.NetworkRunner;
            }
        }

        private void OnStartRespawn(IEventBusArgs args)
        {
            if (args is StartRespawnEventArgs startRespawnEventArgs)
            {
                SetRespawnTime(startRespawnEventArgs.RespawnAt);

                if (_selectWeaponButtons.Count == 0)
                {
                    foreach (var weapon in startRespawnEventArgs.AvailableWeapons)
                    {
                        var weaponButton = Instantiate(selectWeaponButtonPrefab, mainWeaponsParent);
                        _selectWeaponButtons.Add(weaponButton);
                        weaponButton.SetWeapon(weapon.Icon, weapon.ID);
                        weaponButton.OnClick += OnWeaponSelected;
                    }
                }
                // foreach (var utilityItem in WeaponService.Instance.GetAllWeapons())
                // {
                //     var weaponButton = Instantiate(selectWeaponButtonPrefab, mainWeaponsParent);
                //     weaponButton.SetWeapon(weapon.Icon, weapon.ID);
                //     weaponButton.OnClick += OnWeaponSelected;
                // }
            }
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