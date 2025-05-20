using Core.MatchmakingComponents;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MatchFinder matchFinder;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Button findMatchButton;

        public void Start()
        {
            findMatchButton.onClick.AddListener(matchFinder.FindMatch);
        }
    }
}