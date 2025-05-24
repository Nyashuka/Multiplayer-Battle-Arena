using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories.UI
{
    public class MatchTimerUIFactory
    {
        private MatchTimerUI _matchTimerUIPrefab;

        public MatchTimerUIFactory(MatchTimerUI matchTimerUIPrefab)
        {
            _matchTimerUIPrefab = matchTimerUIPrefab;
        }

        public MatchTimerUI Create(Transform parent)
        {
            return Object.Instantiate(_matchTimerUIPrefab, parent);
        }
    }
}