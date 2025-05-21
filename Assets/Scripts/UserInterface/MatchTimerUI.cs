using System.Collections;
using Core.MatchmakingComponents;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class MatchTimerUI : MonoBehaviour
    {
        [SerializeField] private MatchTimer matchTimer;
        [SerializeField] private TMP_Text timerText;

        private void Start()
        {
            StartCoroutine(UpdateTimerCoroutine());
        }

        private IEnumerator UpdateTimerCoroutine()
        {
            yield return new WaitForSeconds(15f);
            
            while (matchTimer && !matchTimer.HasEnded)
            {
                float? remaining = matchTimer.GetRemainingTime();
                if (remaining.HasValue)
                {
                    int minutes = Mathf.FloorToInt(remaining.Value / 60f);
                    int seconds = Mathf.FloorToInt(remaining.Value % 60f);
                    timerText.text = $"{minutes:D2}:{seconds:D2}";
                }
                else
                {
                    timerText.text = "--:--";
                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}