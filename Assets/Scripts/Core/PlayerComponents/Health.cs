using System;
using Data;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class Health : IDamagable, IHealable, IHealthSource
    {
        private readonly int _maxHealth;
        
        public int CurrentHealth { get; private set; }
        public int MaxHealth => _maxHealth;
        public bool IsAlive => CurrentHealth > 0;
        
        public event Action<int> HealthChanged;
        public event Action DeathEvent;

        public Health(int maxHealth)
        {
            _maxHealth = maxHealth;
            SetHealth(maxHealth);
        }
        
        private void SetHealth(int value)
        {
            CurrentHealth = value;
            HealthChanged?.Invoke(CurrentHealth);
    
            if (!IsAlive)
                DeathEvent?.Invoke();
        }

        public void TakeDamage(DamageData data)
        {
            if (data.Damage <= 0 || !IsAlive)
                return;

            SetHealth(Mathf.Max(0, CurrentHealth - data.Damage));
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || !IsAlive) return;

            SetHealth(Mathf.Min(_maxHealth, CurrentHealth + amount));
        }
    }
}