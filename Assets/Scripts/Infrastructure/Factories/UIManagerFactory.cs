using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories
{
    public class UIManagerFactory
    {
        private readonly UIManager _uiManagerPrefab;

        public UIManagerFactory(UIManager uiManagerPrefab)
        {
            _uiManagerPrefab = uiManagerPrefab;
        }

        public UIManager Create()
        {
            return Object.Instantiate(_uiManagerPrefab);
        }
    }
}