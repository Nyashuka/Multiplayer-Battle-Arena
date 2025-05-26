using UnityEngine;

namespace UserInterface.MatchUI.HUDElements
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class HUDElement : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
        }

        public void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
        }
    }
}