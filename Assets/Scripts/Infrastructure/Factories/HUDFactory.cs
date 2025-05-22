using UnityEngine;
using UserInterface;

namespace Infrastructure.Factories
{
    public class HUDFactory
    {
        private readonly Transform _canvasTransform;
        private readonly HUDManager _hudManagerPrefab;

        public HUDFactory(Transform canvasTransform, HUDManager hudManagerPrefab)
        {
            _canvasTransform = canvasTransform;
            _hudManagerPrefab = hudManagerPrefab;
        }

        public HUDManager Create()
        {
            return Object.Instantiate(_hudManagerPrefab, _canvasTransform);
        }
    }
}