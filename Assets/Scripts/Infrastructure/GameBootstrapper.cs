using System;
using ScriptableObjects;
using Services.ServiceLocatorModule;
using Services.ServiceLocatorModule.Abstract;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour, IService
    {
        [SerializeField] private GameBootstrapperConfig config;
        private bool _ran = false;
        
        public void Awake()
        {
            try
            {
                var gameBootstrapper = ServiceLocator.Instance.GetService<GameBootstrapper>();
                gameBootstrapper.Run();
            }
            catch (ArgumentException e)
            {
                ServiceLocator.Instance.Register(this);
                Run();
            }
            
        }

        private void Run()
        {
            if (!_ran)
            {
                DontDestroyOnLoad(this);
                RegisterAudioService();
            }
            
            _ran = true;
            
            SceneManager.LoadScene(config.StartScene);
        }

        private void RegisterAudioService()
        {
            var audioService = Instantiate(config.AudioServicePrefab);
            DontDestroyOnLoad(audioService.gameObject);
            ServiceLocator.Instance.Register(audioService);
        }
    }
}