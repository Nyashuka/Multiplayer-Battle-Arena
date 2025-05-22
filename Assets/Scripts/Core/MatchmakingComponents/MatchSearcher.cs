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
        private const int MaxPlayers = 2;

        private bool _isSearching = false;
        private CancellationTokenSource _cts;

        public event Action OnMatchCreated;
        public event Action OnMatchJoined;
        public event Action<string> OnError;
        public event Action OnSearchCancelled;

        public MatchSearcher(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner ?? throw new ArgumentNullException(nameof(networkRunner));
        }

        public async Task FindMatchAsync()
        {
            if (_isSearching)
            {
                OnError?.Invoke("Search already in progress.");
                return;
            }

            _isSearching = true;
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

                using (_cts.Token.Register(() => {
                           // Якщо потрібна додаткова логіка зупинки, викликати тут
                           // Наприклад: _networkRunner.Shutdown();
                       }))
                {
                    var startGameResult = await startGameTask;

                    if (_cts.IsCancellationRequested)
                    {
                        OnSearchCancelled?.Invoke();
                        return;
                    }

                    if (!startGameResult.Ok)
                    {
                        OnError?.Invoke($"Error: {startGameResult.ErrorMessage}");
                        return;
                    }

                    if (_networkRunner.IsServer)
                    {
                        OnMatchCreated?.Invoke();
                    }
                    else
                    {
                        OnMatchJoined?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_cts.IsCancellationRequested)
                    OnError?.Invoke($"Creating/connection room error: {ex.Message}");
            }
            finally
            {
                _isSearching = false;
                _cts.Dispose();
                _cts = null;
            }
        }

        public void CancelSearch()
        {
            if (_isSearching && _cts is { IsCancellationRequested: false })
            {
                _cts.Cancel();
            }
        }

        private SceneRef CreateMatchmakingScene()
        {
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            return scene.IsValid ? scene : SceneRef.None;
        }
    }
}