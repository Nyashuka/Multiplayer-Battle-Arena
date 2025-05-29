using Data;

namespace Core.PlayerComponents.HealthComponent
{
    public interface IDamagable
    {
        void TakeDamage(DamageData data);
    }
}