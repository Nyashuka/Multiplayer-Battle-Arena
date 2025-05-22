using System;
using Data;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class NetworkHealth : NetworkBehaviour, IDamagable, IHealable, IHealthSource
    {
        [Networked] public PlayerRef Owner { get; set; }
        [Networked] private PlayerRef LastAttacker { get; set; }
        [Networked] private int NetworkHealthValue { get; set; }
        public Health Health { get; private set; }

        [SerializeField] private int maxHealth = 100;

        public event Action<DeathData> DeathEvent;
        public event Action<int> HealthChanged;
        public int CurrentHealth => NetworkHealthValue;
        public int MaxHealth => Health.MaxHealth;
        
        public override void Spawned()
        {
            Health = new Health(maxHealth);
            OnHealthChanged(Health.CurrentHealth);

            Health.HealthChanged += OnHealthChanged;
            Health.DeathEvent += OnDeathEvent;
        }

      
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyHealthChanged()
        {
            HealthChanged?.Invoke(NetworkHealthValue);
        } 
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDeathEvent()
        {
            var deathData = new DeathData()
            {
                Killer = LastAttacker,
                Victim = Owner
            };
            
            DeathEvent?.Invoke(deathData);
            
            GameEventBus.Instance.RaiseEvent(
                    GameEventDefinitions.PlayerDeath, 
                    new PlayerKilledEventArgs(deathData)
                );
        } 
        
        private void OnHealthChanged(int value)
        {
            if (!HasStateAuthority) return;
            
            NetworkHealthValue = value;
            HealthChanged?.Invoke(NetworkHealthValue);
            Rpc_NotifyHealthChanged();
        }
        
        private void OnDeathEvent()
        {
            if(!HasStateAuthority) return;
            
            Rpc_NotifyDeathEvent();
        }

        public void TakeDamage(DamageData data)
        {
            if (!HasStateAuthority || !Health.IsAlive)
                return;

            LastAttacker = data.Attacker;
            Health.TakeDamage(data);
        }

        public void Heal(int amount)
        {
            if (!HasStateAuthority || !Health.IsAlive)
                return;

            Health.Heal(amount);
        }
    }
}