using System;
using System.Collections;
using Core.MatchmakingComponents;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private TMP_InputField playerCountInputField;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private SmartButton findMatchButton;

        private float _searchingTime;
        private bool _isSearching;
        
        private void Start()
        {
            timer.text = "";
            _searchingTime = 0;
            playerCountInputField.text = "3";
            findMatchButton.onClick.AddListener(OnMatchButtonClicked);
        }

        private void OnMatchButtonClicked()
        {
            if (!_isSearching)
            {
                _isSearching = true;
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.StartMatchSearchRequested,
                    new StartMatchSearchEventArgs(Convert.ToInt32(playerCountInputField.text)));
                StartCoroutine(CalcMatchTime());
                findMatchButton.SetText("Stop");
                return;
            }
            
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.StopMatchSearchRequested,
                new EmptyEventArgs());
            _isSearching = false;
            findMatchButton.SetText("Find Match");
            StopCoroutine(CalcMatchTime());
        }

        private string TimeToText(float time)
        {
            return $"{Mathf.FloorToInt(time/60f):D2}:{Mathf.FloorToInt(time%60f):D2}";
        }
        
        private IEnumerator CalcMatchTime()
        {
            while (_isSearching)
            {
                timer.text = TimeToText(_searchingTime);
                _searchingTime += 1;
                yield return new WaitForSeconds(1);
            }

            _searchingTime = 0;
            timer.text = "";
        }
    }
}