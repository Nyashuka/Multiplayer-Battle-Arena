using TMPro;
using UnityEngine;

namespace UserInterface.MatchUI.UIPages
{
    public class ScoreBoardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text killsText;
        [SerializeField] private TMP_Text deathsText;
        [SerializeField] private TMP_Text KDText;
        [SerializeField] private TMP_Text scoreText;

        public void Initialize(string playerName, int kills, int deaths, float kd, int score)
        {
            playerNameText.text = playerName;
            killsText.text = kills.ToString();
            deathsText.text = deaths.ToString();
            KDText.text = kd.ToString();
            scoreText.text = score.ToString();
        }

        public void SetBold()
        {
            playerNameText.fontStyle |= FontStyles.Bold;
            killsText.fontStyle |= FontStyles.Bold;
            deathsText.fontStyle |= FontStyles.Bold;
            KDText.fontStyle |= FontStyles.Bold;
            scoreText.fontStyle |= FontStyles.Bold;
        }
    }
}