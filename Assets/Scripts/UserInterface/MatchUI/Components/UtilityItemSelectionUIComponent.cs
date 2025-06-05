using System;
using System.Collections.Generic;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;

namespace UserInterface.MatchUI.Components
{
    public class UtilityItemSelectionUIComponent : MonoBehaviour
    {
        [SerializeField] private SelectWeaponButton selectWeaponButtonPrefab;
        [SerializeField] private RectTransform utilityItemsParent;
        
        private readonly List<SelectWeaponButton> _selectUtilityItemButtons = new();

        public void Initialize()
        {
            if (_selectUtilityItemButtons.Count == 0)
            {
                var utilities = ServiceLocator.Instance
                    .GetService<UtilityItemsDatabaseService>()
                    .GetAll();
                foreach (var utilityItem in utilities)
                {
                    var weaponButton = Instantiate(selectWeaponButtonPrefab, utilityItemsParent);
                    _selectUtilityItemButtons.Add(weaponButton);
                    weaponButton.SetWeapon(utilityItem.Icon, utilityItem.Id);
                    weaponButton.OnClick += OnUtilityItemSelected;
                }
            }
        }

        private void OnUtilityItemSelected(string id)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.UtilityItemRequested, new UtilityItemRequestedEventArgs(id));
            Debug.Log("Raised request utility " + id);
        }
    }
}