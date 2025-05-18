using System;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class Health : IDamagable, IHealable, IHealthSource
    {
        public bool IsAlive => CurrentHealth > 0;
        
        private int _maxHealth = 100;
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get => _maxHealth; }
        
        public event Action<int> HealthChanged;
        public event Action Death;

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
                Death?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || !IsAlive)
                return;

            SetHealth(Mathf.Max(0, CurrentHealth - damage));
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || !IsAlive) return;

            SetHealth(Mathf.Min(_maxHealth, CurrentHealth + amount));
        }
    }
}