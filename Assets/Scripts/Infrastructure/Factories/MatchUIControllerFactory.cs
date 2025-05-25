using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories
{
    public class MatchUIControllerFactory
    {
        private readonly MatchUIController _matchUIControllerPrefab;

        public MatchUIControllerFactory(MatchUIController matchUIControllerPrefab)
        {
            _matchUIControllerPrefab = matchUIControllerPrefab;
        }

        public MatchUIController Create(Transform parent)
        {
            return Object.Instantiate(_matchUIControllerPrefab, parent);
        }
    }
}