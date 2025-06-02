namespace Services.EventBus
{
    public static class GameEventDefinitions
    {
        public static string PlayerDeath => "PlayerDeath";
        public static string PlayerMatchStatsChanged => "PlayerMatchStatsChanged";
        public static string StartMatchSearchRequested => "StartMatchSearchRequested";
        public static string StopMatchSearchRequested => "StopMatchSearchRequested";
        public static string PlayerRespawnStarted => "PlayerRespawnStarted";
        public static string PlayerRespawned => "PlayerRespawned";
        public static string PlayerInitialSpawned => "PlayerInitialSpawned";
        public static string MatchStarted => "MatchStarted";
        public static string WeaponRequested => "WeaponRequested";
        public static string WeaponReceived => "WeaponReceived";
        public static string UtilityItemRequested => "UtilityItemRequested";
        public static string MatchTimerChanged => "MatchTimerChanged";
        public static string MatchStateChanged => "MatchStateChanged";
        public static string LeaderboardDataAvailable => "LeaderboardDataAvailable";
        public static string UtilityItemReceived => "UtilityItemReceived";
    }
}