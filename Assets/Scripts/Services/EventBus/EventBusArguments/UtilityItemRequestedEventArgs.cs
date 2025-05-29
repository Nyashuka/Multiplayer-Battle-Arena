namespace Services.EventBus.EventBusArguments
{
    public class UtilityItemRequestedEventArgs : IEventBusArgs
    {
        public UtilityItemRequestedEventArgs(string utilityItemId)
        {
            UtilityItemId = utilityItemId;
        }

        public string UtilityItemId { get; private set; }
    }
}