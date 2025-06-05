using System;
using Core.PlayerComponents.HealthComponent;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.HUDElements
{
    public class HealthBar : HUDElement
    {
        [SerializeField] private Slider healthSlider;
        
        private NetworkHealth _networkHealth;

        public void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerInitialSpawned, OnPlayerSpawned, true);            
        }

        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerInitialSpawned, OnPlayerSpawned);            
        }

        private void OnPlayerSpawned(IEventBusArgs args)
        {
            if (args is PlayerSpawnedEventArgs playerSpawnedEventArgs)
            {
                var health = playerSpawnedEventArgs.Player.NetworkHealth;
                AttachHealth(health);
            }
        }

        private void AttachHealth(NetworkHealth health)
        {
            healthSlider.maxValue = health.MaxHealth;
            healthSlider.value = health.CurrentHealth;
            healthSlider.minValue = 0;

            health.HealthChanged += OnHealthChanged;
            
            _networkHealth = health;
        }

        private void OnHealthChanged(int newHealth)
        {
            healthSlider.value = newHealth;
        }
    }
}