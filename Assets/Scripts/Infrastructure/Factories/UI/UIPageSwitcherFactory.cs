using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories.UI
{
    public class UIPageSwitcherFactory
    {
        private readonly UIPageSwitcher _uiPageSwitcherPrefab;

        public UIPageSwitcherFactory(UIPageSwitcher uiPageSwitcherPrefab)
        {
            _uiPageSwitcherPrefab = uiPageSwitcherPrefab;
        }

        public UIPageSwitcher Create()
        {
            return Object.Instantiate(_uiPageSwitcherPrefab);
        }
    }
}