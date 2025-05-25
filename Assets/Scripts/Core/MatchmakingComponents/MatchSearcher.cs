using System;
using System.Threading;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.MatchmakingComponents
{
    public class MatchSearcher 
    {
        private readonly NetworkRunner _networkRunner;
        private const int MaxPlayers = 3;

        public bool IsSearching { get; private set;  } = false;
        private CancellationTokenSource _cts;

        public MatchSearcher(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner ?? throw new ArgumentNullException(nameof(networkRunner));
        }

        public async Task FindMatchAsync()
        {
            if (IsSearching)
            {
                return;
            }

            IsSearching = true;
            _cts = new CancellationTokenSource();

            try
            {
                var sceneManager = _networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
                _networkRunner.ProvideInput = true;

                var startArgs = new StartGameArgs
                {
                    GameMode = GameMode.AutoHostOrClient,
                    EnableClientSessionCreation = true,
                    PlayerCount = MaxPlayers,
                    IsVisible = true,
                    IsOpen = true,
                    SceneManager = sceneManager,
                    Scene = CreateMatchmakingScene()
                };

                var startGameTask = _networkRunner.StartGame(startArgs);

                var startGameResult = await startGameTask;

                if (!startGameResult.Ok)
                {
                    IsSearching = false;
                }
            }
            catch (Exception ex)
            {
                if (!_cts.IsCancellationRequested)
                    Debug.Log($"Creating/connection room error: {ex.Message}");
                
                IsSearching = false;
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        public void CancelSearch()
        {
            if (IsSearching && _cts is { IsCancellationRequested: false })
            {
                _cts.Cancel();
                IsSearching = false;
            }
        }

        private SceneRef CreateMatchmakingScene()
        {
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            return scene.IsValid ? scene : SceneRef.None;
        }
    }
}