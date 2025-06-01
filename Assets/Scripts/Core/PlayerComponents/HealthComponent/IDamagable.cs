using Data;
using Fusion;

namespace Core.PlayerComponents.HealthComponent
{
    public interface IDamagable
    {
        void TakeDamage(DamageData data);
        PlayerRef Owner { get; set; }
    }
}