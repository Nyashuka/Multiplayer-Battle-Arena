using System;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.MatchmakingComponents
{
    public class MatchFinder : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef matchHostControllerPrefab;
        private const string FindMatchmakingRoomName = "FindMatchmaking";
        [SerializeField] private NetworkRunner networkRunner;
        private const int MaxPlayers = 2;
        
        // private bool started = false; 
        
        public void FindMatch()
        {
            JoinOrCreateRoom();
        }
        
        public async void JoinOrCreateRoom()
        {
            var sceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            networkRunner.ProvideInput = true;
            var startArgs = new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,        // Стане хостом, якщо не знайде кімнату
                // MatchmakingMode = MatchmakingMode.FillRoom,  // Спробує приєднатися до існуючої
                EnableClientSessionCreation = true,          // Якщо не знайде — створить нову
                PlayerCount = MaxPlayers,                             // Максимум 5 гравців у сесії
                SessionName = null,                          // Автоматична назва сесії
                IsVisible = true,                            // Інші можуть побачити цю кімнату
                IsOpen = true,                                // Інші можуть приєднуватись
                SceneManager = sceneManager,
                Scene = CreateMatchmakingScene()
            };

            try
            {
                var startGameResult = await networkRunner.StartGame(startArgs);
                if (!startGameResult.Ok)
                {
                    Debug.Log("Error " + startGameResult.ErrorMessage);
                    return;
                }
                if (networkRunner.IsServer)
                {
                    Debug.Log("Created matchmaking room.");
                    // var matchHostController = networkRunner.Spawn(matchHostControllerPrefab).GetComponent<MatchHostController>();
                    // matchHostController.Initialize(networkRunner);
                }
                else
                {
                    Debug.Log("Connected to matchmaking room.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Creating/connection room error: " + ex.Message);
            }
        }
        //
        // public async void FindMatch()
        // {
        //     if (started)
        //     {
        //         Debug.Log("Already started");
        //         return;
        //     }
        //     
        //     started = true;
        //     
        //     _networkRunner.ProvideInput = true;
        //
        //     var result = await _networkRunner.StartGame(new StartGameArgs
        //     {
        //         GameMode = GameMode.AutoHostOrClient,
        //         SessionName = FindMatchmakingRoomName,
        //         SceneManager = _networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>(),
        //         Scene = CreateMatchmakingScene()
        //     });
        //
        //     if (result.Ok && _networkRunner.IsServer)
        //     {
        //         Debug.Log("Я став Host, створюю логіку для матчмейкінгу...");
        //         var matchHostController = Instantiate(matchHostControllerPrefab);
        //     }
        // }
        //
        public SceneRef CreateMatchmakingScene()
        {
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }
            
            return scene;
        }
    }
}