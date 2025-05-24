using UnityEngine;

namespace Infrastructure.Factories.UI
{
    public class CanvasFactory
    {
        private readonly Canvas _canvasPrefab;

        public CanvasFactory(Canvas canvasPrefab)
        {
            _canvasPrefab = canvasPrefab;
        }

        public Canvas Create()
        {
            return Object.Instantiate(_canvasPrefab);
        } 
    }
}