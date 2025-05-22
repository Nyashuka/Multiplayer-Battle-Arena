using System;
using Core.MatchmakingComponents;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        public event Action FindMatchRequested;

        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Button findMatchButton;

        private void Start()
        {
            findMatchButton.onClick.AddListener(OnFindMatchClicked);
        }

        private void OnFindMatchClicked()
        {
            FindMatchRequested?.Invoke();
        }
    }
}