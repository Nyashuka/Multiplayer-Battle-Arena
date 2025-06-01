using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    public class ScoreBoard : UIPage
    {
        [SerializeField] private ScoreBoardItem scoreBoardItemPrefab;
        [SerializeField] private Transform scoreBoardItemContainer;
        
        private ScoreBoardItem _header;
        private readonly List<ScoreBoardItem> _scoreBoardItems = new();

        private void Start()
        {
            _header = Instantiate(scoreBoardItemPrefab, scoreBoardItemContainer);
            _header.SetBold();
        }

        public void Initialize(NetworkStatsData[] data)
        {
            foreach (var scoreBoardItem in _scoreBoardItems)
            {
                Destroy(scoreBoardItem.gameObject);
            }
            
            foreach (var record in data)
            {
                var scoreBoardItem = Instantiate(scoreBoardItemPrefab, scoreBoardItemContainer);
                scoreBoardItem.Initialize(record.Owner.ToString(), record.Kills, record.Deaths, record.Kd, record.Points);
                _scoreBoardItems.Add(scoreBoardItem);
            }
        }
    }
}