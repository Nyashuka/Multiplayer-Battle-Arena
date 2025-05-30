using Infrastructure.MatchStates;

namespace Services.EventBus.EventBusArguments
{
    public class MatchStateChangedEventArgs : IEventBusArgs
    {
        public MatchStateEnum State { get; }

        public MatchStateChangedEventArgs(MatchStateEnum state)
        {
            State = state;
        }
    }
}