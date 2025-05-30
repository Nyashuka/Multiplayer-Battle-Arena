namespace Services.EventBus
{
    public static class GameEventDefinitions
    {
        public const string PlayerDeath = "PlayerDeath";
        public const string StatisticsChanged = "StatisticsChanged";
        public const string StartMatchSearchRequested = "StartMatchSearchRequested";
        public const string StopMatchSearchRequested = "StopMatchSearchRequested";
        public const string StartRespawn = "StartRespawn";
        public const string PlayerRespawned = "PlayerRespawned";
        public const string PlayerSpawned = "PlayerSpawned";
        public const string MatchStarted = "MatchStarted";
        public const string WeaponRequested = "WeaponRequested";
        public const string UtilityItemRequested = "UtilityItemRequested";
        public const string MatchTimerChanged = "MatchTimerChanged";
        public const string MatchStateChanged = "MatchStateChanged";
    }
}