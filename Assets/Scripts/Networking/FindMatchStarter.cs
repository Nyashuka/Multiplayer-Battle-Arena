using System;
using Fusion;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class FindMatchStarter
    {
        public async void FindMatchAsync(NetworkRunner networkRunner, int playersCount)
        {
            var sceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            networkRunner.ProvideInput = true;

            var startArgs = new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                EnableClientSessionCreation = true,
                PlayerCount = playersCount,
                IsVisible = true,
                IsOpen = true,
                SceneManager = sceneManager,
                Scene = CreateMatchmakingScene()
            };

            var startGameTask = networkRunner.StartGame(startArgs);
            await startGameTask;
        }

        private SceneRef CreateMatchmakingScene()
        {
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            return scene.IsValid ? scene : SceneRef.None;
        } 
    }
}