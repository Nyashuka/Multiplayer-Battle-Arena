using System;

namespace Core.PlayerComponents
{
    public interface IHealthSource
    {
        event Action<int> HealthChanged; 
        int CurrentHealth { get; }
        int MaxHealth { get; }
    }
}