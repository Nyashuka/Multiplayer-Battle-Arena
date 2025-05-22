using UnityEngine;
using UserInterface;

namespace Infrastructure.Factories
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