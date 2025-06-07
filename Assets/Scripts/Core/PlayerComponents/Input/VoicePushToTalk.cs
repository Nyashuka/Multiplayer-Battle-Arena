using Photon.Voice.Unity;
using UnityEngine;

namespace Core.PlayerComponents.Input
{
    public class VoicePushToTalk : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        [SerializeField] private KeyCode keyCode = KeyCode.C;

        private void Start()
        {
            recorder.TransmitEnabled = false;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                recorder.TransmitEnabled = true;
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.C))
            {
                recorder.TransmitEnabled = false;
            }
        }
    }
}