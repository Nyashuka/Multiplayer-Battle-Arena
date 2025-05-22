using Core.MatchmakingComponents.ScoreSystem;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using UserInterface;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour
    {
        private MatchScore _matchScore;
        private MatchStatistic _matchStatistic;
        
        public override void Spawned()
        {
            _matchScore = new MatchScore();
            _matchStatistic = new MatchStatistic();
            
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
        }

        public void StartMatch()
        {
    
        }

        private void OnPlayerDeath(IEventBusArgs args)
        {
            if (args is PlayerKilledEventArgs playerKilledEventArgs)
            {
                _matchStatistic.AddKill(playerKilledEventArgs.DeathData.Killer);
                _matchStatistic.AddDeath(playerKilledEventArgs.DeathData.Victim);
                _matchScore.AddScore(playerKilledEventArgs.DeathData.Killer, 1);
                    
                GameEventBus.Instance.RaiseEvent(
                    GameEventDefinitions.StatisticsChanged, 
                    new StatisticsChangedEventArgs(Runner.LocalPlayer, _matchStatistic)
                );
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyNewPlayerScore(PlayerRef player, int score)
        {
            if(HasStateAuthority) return;

            Debug.Log(player + " | score: " + score);
            _matchScore.SetScore(player, score);
        }
    }
}