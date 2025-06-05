using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MatchUI.UIPages
{
    public class ScoreBoard : UIPage
    {
        [SerializeField] private ScoreBoardItem scoreBoardItemPrefab;
        [SerializeField] private Transform scoreBoardItemContainer;
        [SerializeField] private Button leftMatchButton;
        
        private ScoreBoardItem _header;
        private readonly List<ScoreBoardItem> _scoreBoardItems = new();

        private void Start()
        {
            _header = Instantiate(scoreBoardItemPrefab, scoreBoardItemContainer);
            _header.SetBold();
            
            leftMatchButton.onClick.AddListener(OnLeftMatchClicked);
        }

        private void OnLeftMatchClicked()
        {
            MainNetworkRunnerHandler.Instance.LeftMatch();
        }

        public void Initialize(NetworkStatsData[] data)
        {
            foreach (var scoreBoardItem in _scoreBoardItems)
            {
                Destroy(scoreBoardItem.gameObject);
            }

            var sortedData = data.OrderByDescending(x => x.Points).ToList();
            foreach (var record in sortedData)
            {
                var scoreBoardItem = Instantiate(scoreBoardItemPrefab, scoreBoardItemContainer);
                scoreBoardItem.Initialize(record.Owner.ToString(), record.Kills, record.Deaths, record.Kd, record.Points);
                _scoreBoardItems.Add(scoreBoardItem);
            }
        }
    }
}