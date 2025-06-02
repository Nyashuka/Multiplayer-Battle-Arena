namespace Services.EventBus.EventBusArguments
{
    public class WeaponReceivedEventArgs : IEventBusArgs
    {
        public WeaponReceivedEventArgs(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}