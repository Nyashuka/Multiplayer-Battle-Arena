namespace Services.EventBus
{
    public static class GameEventDefinitions
    {
        public const string PlayerDeath = "PlayerDeath";
        public const string StatisticsChanged = "StatisticsChanged";
        public const string StartMatchSearchRequested = "StartMatchSearchRequested";
        public const string StopMatchSearchRequested = "StopMatchSearchRequested";
    }
}