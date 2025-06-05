using System;
using System.Collections;
using System.Linq;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    public class MainMenu : UIPage
    {
        [SerializeField] private TMP_InputField playerCountInputField;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private SmartButton findMatchButton;

        private float _searchingTime;
        private bool _isSearching;

        private void OnClose()
        {
            _searchingTime = 0;
            _isSearching = false;
            StopCoroutine(CalcMatchTime());
            findMatchButton.SetText("Find Match");
            timer.text = "";
        }

        private void OnEnable()
        {
            OnCloseEvent += OnClose;
        }

        private void OnDisable()
        {
            OnCloseEvent -= OnClose;
        }

        private void Start()
        {
            timer.text = "";
            _searchingTime = 0;
            playerCountInputField.text = "3";
            findMatchButton.onClick.AddListener(OnMatchButtonClicked);
            playerCountInputField.onValueChanged.AddListener(OnPlayerCountChanged);
            playerCountInputField.onEndEdit.AddListener(OnPlayerCountEndEdit);        
        }

        private void OnPlayerCountEndEdit(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                playerCountInputField.text = "1";
                return;
            }

            if (int.TryParse(input, out int value))
            {
                value = Mathf.Clamp(value, 1, 20);
                playerCountInputField.text = value.ToString();
            }
            else
            {
                playerCountInputField.text = "1";
            }
        }

        private void OnPlayerCountChanged(string input)
        {
            string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

            if (input != digitsOnly)
            {
                playerCountInputField.text = digitsOnly;
            }        
        }

        private void OnMatchButtonClicked()
        {
            if (!_isSearching)
            {
                RequestStartSearchMatch();
                return;
            }
            
            RequestStopMatchSearch();
        }

        private void RequestStartSearchMatch()
        {
            _isSearching = true;
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.StartMatchSearchRequested,
                new StartMatchSearchEventArgs(Convert.ToInt32(playerCountInputField.text)));
            StartCoroutine(CalcMatchTime());
            findMatchButton.SetText("Stop");
        }

        private void RequestStopMatchSearch()
        {
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