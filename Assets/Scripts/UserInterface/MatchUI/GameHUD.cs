using System.Collections.Generic;
using System.Linq;
using Core.PlayerComponents;
using UnityEngine;
using UserInterface.MatchUI.HUDElements;

namespace UserInterface.MatchUI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private Canvas hudCanvas;
        [SerializeField] private List<HUDElement> hudElements;

        public T GetElement<T>() where T : HUDElement
        {
            return hudElements.OfType<T>().FirstOrDefault();
        }

        public void ShowAll()
        {
            foreach (var element in hudElements) element.Show();
        }

        public void HideAll()
        {
            foreach (var element in hudElements) element.Hide();
        }
    }
}