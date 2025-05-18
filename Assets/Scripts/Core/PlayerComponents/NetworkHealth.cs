using System;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class NetworkHealth : NetworkBehaviour, IDamagable, IHealable, IHealthSource
    {
        [Networked] private int NetworkHealthValue { get; set; }
        public Health Health { get; private set; }

        [SerializeField] private int maxHealth = 100;
        
        public event Action<int> HealthChanged;
        public int CurrentHealth => NetworkHealthValue;
        public int MaxHealth => Health.MaxHealth;
        
        public override void Spawned()
        {
            Health = new Health(maxHealth);
            OnHealthChanged(Health.CurrentHealth);

            Health.HealthChanged += OnHealthChanged;
            Health.Death += OnDeath;
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

        private void OnDeath()
        {
            
        }

        public void TakeDamage(int damage)
        {
            if (!HasStateAuthority || !Health.IsAlive)
                return;

            Health.TakeDamage(damage);
        }

        public void Heal(int amount)
        {
            if (!HasStateAuthority || !Health.IsAlive)
                return;

            Health.Heal(amount);
        }
    }
}