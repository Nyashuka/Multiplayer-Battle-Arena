using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserInterface.MatchUI.UIPages;

namespace UserInterface.MatchUI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class UIPageSwitcher : MonoBehaviour
    {
        [SerializeField] private List<UIPage> uiPages;

        private UIPage _currentPage;

        private T GetPage<T>()
        {
            return uiPages.OfType<T>().FirstOrDefault();
        }
        
        public T SwitchPage<T>() where T : UIPage
        {
            if(_currentPage != null)
                _currentPage.Close();
            
            _currentPage = GetPage<T>();
            _currentPage.Open();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return (T)_currentPage;
        }

        public void ClosePage()
        {
            if(_currentPage != null)
                _currentPage.Close();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}