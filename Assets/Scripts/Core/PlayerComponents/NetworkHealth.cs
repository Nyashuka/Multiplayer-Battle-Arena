using System;
using Data;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class NetworkHealth : NetworkBehaviour, IDamagable, IHealable, IHealthSource
    {
        [Networked] public PlayerRef Owner { get; set; }
        public PlayerRef LastAttacker { get; private set; }
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

        private void OnHealthChanged(int value)
        {
            if (HasStateAuthority)
            {
                NetworkHealthValue = value;
                HealthChanged?.Invoke(NetworkHealthValue);
                Rpc_NotifyHealthChanged();
            }
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyHealthChanged()
        {
            HealthChanged?.Invoke(NetworkHealthValue);
        } 

        private void OnDeathEvent()
        {
            DeathEvent?.Invoke(new DeathData()
            {
                Killer = LastAttacker,
                Victim = Owner
            });
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