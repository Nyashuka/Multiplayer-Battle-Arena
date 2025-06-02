namespace Services.EventBus.EventBusArguments
{
    public class WeaponRequestedEventArgs : IEventBusArgs
    {
        public WeaponRequestedEventArgs(string weaponId)
        {
            WeaponId = weaponId;
        }

        public string WeaponId { get; private set; }
    }
}