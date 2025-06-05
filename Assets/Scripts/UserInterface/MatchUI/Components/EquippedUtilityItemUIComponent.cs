using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.Components
{
    public class EquippedUtilityItemUIComponent : MonoBehaviour
    {
        [SerializeField] private Image utilityItemIcon;
        
        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.UtilityItemReceived, OnUtilityItemReceived, true);
        }
                
        private void OnDisable()
        {
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
    }
}