using System;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.MatchUI.Components;

namespace UserInterface.MatchUI.UIPages
{
    public class StartMatchScreen : UIPage
    {
        [SerializeField] private MainWeaponSelectionUIComponent weaponSelectionComponent;
        [SerializeField] private UtilityItemSelectionUIComponent utilityItemSelectionComponent;
        [SerializeField] private Button confirmButton;

        public void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            Close();
        }

        public void Initialize()
        {
            weaponSelectionComponent.Initialize();
            utilityItemSelectionComponent.Initialize();
        }
    }
}