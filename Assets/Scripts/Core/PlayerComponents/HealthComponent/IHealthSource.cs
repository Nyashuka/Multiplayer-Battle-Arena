using System;

namespace Core.PlayerComponents.HealthComponent
{
    public interface IHealthSource
    {
        event Action<int> HealthChanged; 
        int CurrentHealth { get; }
        int MaxHealth { get; }
    }
}