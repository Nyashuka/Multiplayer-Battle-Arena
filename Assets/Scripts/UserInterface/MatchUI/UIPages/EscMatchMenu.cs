using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.UIPages
{
    public class EscMatchMenu : UIPage
    {
        [SerializeField] private Button leftMatchButton;

        public void OnEnable()
        {
            leftMatchButton.onClick.AddListener(OnLeftMatchClicked);
        }

        private void OnLeftMatchClicked()
        {
            MainNetworkRunnerHandler.Instance.LeftMatch();
        }
    }
}