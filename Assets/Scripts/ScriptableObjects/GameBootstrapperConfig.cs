using Services.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects
{
    
    [CreateAssetMenu(menuName = "Game/GameBootstrapperConfig")]
    public class GameBootstrapperConfig : ScriptableObject
    {
        [SerializeField] private string startScene;
        [SerializeField] private AudioService audioServicePrefab;
        
        public string StartScene => startScene;
        public AudioService AudioServicePrefab => audioServicePrefab;
    }
}