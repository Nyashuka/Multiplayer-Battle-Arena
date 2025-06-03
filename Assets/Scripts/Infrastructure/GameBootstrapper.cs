using ScriptableObjects;
using Services.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameBootstrapperConfig config;
        
        public void Awake()
        {
            RegisterAudioService();
            SceneManager.LoadScene(config.StartScene);
        }

        private void RegisterAudioService()
        {
            var audioService = Object.Instantiate(config.AudioServicePrefab);
            DontDestroyOnLoad(audioService.gameObject);
            ServiceLocator.Instance.Register(audioService);
        }
    }
}