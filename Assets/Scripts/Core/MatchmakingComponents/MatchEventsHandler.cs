using Services.EventBus;
using Services.EventBus.EventBusArguments;

namespace Core.MatchmakingComponents
{
    public class MatchEventsHandler
    {
        private MatchEventsHandler(GameEventBus gameEventBus)
        {
            gameEventBus.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
        }

        private void OnPlayerDeath(IEventBusArgs args)
        {
            
        }
    }
}