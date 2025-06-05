
using System;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        public event Action OnOpenEvent;
        public event Action OnCloseEvent;
        
        public bool Opened { get; private set; }
        
        public void Open()
        {
            canvasGroup.alpha = 1;     
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Opened = true;
            OnOpenEvent?.Invoke();
        }

        public void Close()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Opened = false;
            OnCloseEvent?.Invoke();
        }
    }
}