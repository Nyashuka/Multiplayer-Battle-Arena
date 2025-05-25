using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class SmartButton : Button
    {
        [SerializeField] private TMP_Text text;

        public void SetText(string newText)
        {
            text.text = newText;
        }
    }
}