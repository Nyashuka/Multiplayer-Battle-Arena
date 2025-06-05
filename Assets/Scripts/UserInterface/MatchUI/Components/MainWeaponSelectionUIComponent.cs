using System;
using System.Collections.Generic;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;

namespace UserInterface.MatchUI.Components
{
    public class MainWeaponSelectionUIComponent : MonoBehaviour
    {
        [SerializeField] private SelectWeaponButton selectWeaponButtonPrefab;
        [SerializeField] private RectTransform mainWeaponsParent;
        
        private readonly List<SelectWeaponButton> _selectWeaponButtons = new();

        public void Initialize()
        {
            if (_selectWeaponButtons.Count == 0)
            {
                var weapons = ServiceLocator.Instance
                    .GetService<WeaponDatabaseService>()
                    .GetAll();
                
                foreach (var weapon in weapons)
                {
                    var weaponButton = Instantiate(selectWeaponButtonPrefab, mainWeaponsParent);
                    _selectWeaponButtons.Add(weaponButton);
                    weaponButton.SetWeapon(weapon.Icon, weapon.ID);
                    weaponButton.OnClick += OnWeaponSelected;
                }
            }
        }

        private void OnWeaponSelected(string id)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.WeaponRequested, new WeaponRequestedEventArgs(id));
            Debug.Log("Raised request weapon " + id);
        }
    }
}