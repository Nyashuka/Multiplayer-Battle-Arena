using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.Components
{
    public class EquippedWeaponUIComponent : MonoBehaviour
    {
        [SerializeField] private Image mainWeaponIcon;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.WeaponReceived, OnWeaponReceived, true);
        }
        
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.WeaponReceived, OnWeaponReceived);
        }

        private void OnWeaponReceived(IEventBusArgs e)
        {
            if (e is WeaponReceivedEventArgs received)
            {
                var weaponDatabase = ServiceLocator.Instance.GetService<WeaponDatabaseService>();
                var config = weaponDatabase.GetById(received.Id);
                mainWeaponIcon.sprite = config.Icon;
            }
        }
    }
}