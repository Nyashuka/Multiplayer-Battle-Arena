using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.HUDElements
{
    public class EquippedWeaponsHUD : HUDElement
    {
        [SerializeField] private Image mainWeaponIcon;
        [SerializeField] private Image utilityItemIcon;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.WeaponReceived, OnWeaponReceived, true);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.UtilityItemReceived, OnUtilityItemReceived, true);
        }
        
        private void OnDisable()
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.WeaponReceived, OnWeaponReceived);
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.UtilityItemReceived, OnUtilityItemReceived);
        }

        private void OnUtilityItemReceived(IEventBusArgs e)
        {
            if (e is UtilityItemReceivedEventArgs received)
            {
                var utilityItemsDatabase = ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>();
                var config = utilityItemsDatabase.GetById(received.Id);
                utilityItemIcon.sprite = config.Icon;
            }
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