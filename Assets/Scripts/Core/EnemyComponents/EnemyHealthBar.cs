using Core.PlayerComponents;
using Core.PlayerComponents.HealthComponent;
using UnityEngine;
using UnityEngine.UI;

namespace Core.EnemyComponents
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private IHealthSource _healthSource;

        public void Initialize(IHealthSource healthSource)
        {
            _healthSource = healthSource;
            _healthSource.HealthChanged += OnHealthChanged;
            slider.maxValue = _healthSource.MaxHealth;
            slider.value = _healthSource.CurrentHealth;
            slider.minValue = 0;
        }

        private void OnHealthChanged(int points)
        {
            slider.value = points;
        }
    }
}