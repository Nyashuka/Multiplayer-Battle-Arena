namespace Services.EventBus
{
    public static class GameEventDefinitions
    {
        // Player
        public static string PlayerRespawnStarted => "PlayerRespawnStarted";
        public static string PlayerRespawned => "PlayerRespawned";
        public static string PlayerInitialSpawned => "PlayerInitialSpawned";
        public static string PlayerDeath => "PlayerDeath";
        public static string PlayerLeft => "PlayerLeft";
        public static string PlayerLost => "PlayerLost";
        public static string PlayerMatchStatsChanged => "PlayerMatchStatsChanged";
        
        // Match
        public static string StartMatchSearchRequested => "StartMatchSearchRequested";
        public static string StopMatchSearchRequested => "StopMatchSearchRequested";
        public static string MatchTimerChanged => "MatchTimerChanged";
        public static string MatchStateChanged => "MatchStateChanged";
        
        // Weapons
        public static string WeaponRequested => "WeaponRequested";
        public static string WeaponReceived => "WeaponReceived";
        public static string UtilityItemRequested => "UtilityItemRequested";
        public static string UtilityItemReceived => "UtilityItemReceived";
        
        // 
        public static string LeaderboardDataAvailable => "LeaderboardDataAvailable";
    }
}