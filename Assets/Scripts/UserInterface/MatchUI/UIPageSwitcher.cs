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
        
        public void SwitchPage<T>() where T : UIPage
        {
            if(_currentPage != null)
                _currentPage.Close();
            
            _currentPage = GetPage<T>();
            _currentPage.Open();
        }

        public void ClosePage()
        {
            if(_currentPage != null)
                _currentPage.Close();
        }
    }
}