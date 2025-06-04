using System;
using Core.Modifiers;
using Data;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;

namespace Core.PlayerComponents.HealthComponent
{
    public class NetworkHealth : NetworkBehaviour, IDamagable, IHealable, IHealthSource
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int maxlives = 3;
        [SerializeField] private ParticleSystem deathEffectPrefab;
        
        [Networked] public PlayerRef Owner { get; set; }
        [Networked] private PlayerRef LastAttacker { get; set; }
        [Networked] private int NetworkHealthValue { get; set; }
        [Networked] private int NetworkLivesValue { get; set; }
        [Networked] private bool AlreadyDead { get; set; }

        private ModifierStack<int> IncomingDamageModifiers { get; } = new();
        
        public bool IsAlive => NetworkHealthValue > 0;
        public int CurrentHealth => NetworkHealthValue;
        public int CurrentLives => NetworkLivesValue;
        public int MaxHealth => maxHealth;
        
        public event Action<DeathData> DeathEvent;
        public event Action<int> HealthChanged;

        public override void Spawned()
        {
            ResetHealth();
            ResetLives();
        }

        public void ResetHealth()
        {
            if(!HasStateAuthority) return;
            
            AlreadyDead = false;
            NetworkHealthValue = maxHealth;
            OnHealthChanged();
        }
        
        private void ResetLives()
        {
            if(!HasStateAuthority) return;
            
            NetworkLivesValue = maxlives;
        }

        public void AddIncomingDamageModifier(IModifier<int> modifier)
        {
            IncomingDamageModifiers.AddModifier(modifier);
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
            
            ServiceLocator.Instance.GetService<VFXService>()
                .PlayLocalVFX(deathEffectPrefab, transform.position + transform.up, transform.rotation);
            
            Debug.Log("RPC Notify Death Event");
            
            GameEventBus.Instance.RaiseEvent(
                GameEventDefinitions.PlayerDeath, 
                new PlayerDeathEventArgs(deathData)
            );
        } 
        
        private void OnHealthChanged()
        {
            if (!HasStateAuthority) return;
            
            HealthChanged?.Invoke(NetworkHealthValue);
            Rpc_NotifyHealthChanged();
        }
        
        private void Die()
        {
            if(!HasStateAuthority) return;

            AlreadyDead = true;
            
            NetworkLivesValue--;
            Debug.Log(Owner + "  Lives: " + NetworkLivesValue);
            
            Rpc_NotifyDeathEvent();
        }

        public void TakeDamage(DamageData data)
        {
            if (!HasStateAuthority || !IsAlive)
                return;
            
            data.Damage = IncomingDamageModifiers.ApplyModifiers(data.Damage);

            LastAttacker = data.Attacker;
            NetworkHealthValue -= data.Damage;
            OnHealthChanged();

            if (!IsAlive && !AlreadyDead)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (!HasStateAuthority || !IsAlive || amount <= 0)
                return;

            NetworkHealthValue += amount;
            OnHealthChanged();
        }

    }
}