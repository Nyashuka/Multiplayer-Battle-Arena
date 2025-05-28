using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class SelectWeaponButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        private string _itemId;

        public event Action<string> OnClick;
        
        public void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Debug.Log("Clicked");
            OnClick?.Invoke(_itemId);
        }

        public void SetWeapon(Sprite sprite, string itemId)
        {
            if (sprite != null)
            {
                image.sprite = sprite;
            }
            _itemId = itemId;
        }
    }
}