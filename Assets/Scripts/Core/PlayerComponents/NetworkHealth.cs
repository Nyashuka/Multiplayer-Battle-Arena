using Fusion;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class NetworkHealth : NetworkBehaviour, IDamagable, IHealable
    {
        [Networked] private int NetworkHealthValue { get; set; }
        public Health Health { get; private set; }

        [SerializeField] private int maxHealth = 100;
        
        [ContextMenu("Test Damage")]
        private void TestDamage()
        {
            TakeDamage(10);
        }

        public override void Spawned()
        {
            Health = new Health(maxHealth);
            HealthChanged(NetworkHealthValue);

            Health.HealthChanged += HealthChanged;
            Health.Death += OnDeath;
        }

        private void HealthChanged(int value)
        {
            if (HasStateAuthority)
                NetworkHealthValue = value;
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