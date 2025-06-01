using Unity.VisualScripting;

namespace Services.EventBus
{
    public static class GameEventDefinitions
    {
        public static string PlayerDeath => "PlayerDeath";
        public static string PlayerStatsChanged => "PlayerStatsChanged";
        public static string StartMatchSearchRequested => "StartMatchSearchRequested";
        public static string StopMatchSearchRequested => "StopMatchSearchRequested";
        public static string StartRespawn => "StartRespawn";
        public static string PlayerRespawned => "PlayerRespawned";
        public static string PlayerSpawned => "PlayerSpawned";
        public static string MatchStarted => "MatchStarted";
        public static string WeaponRequested => "WeaponRequested";
        public static string UtilityItemRequested => "UtilityItemRequested";
        public static string MatchTimerChanged => "MatchTimerChanged";
        public static string MatchStateChanged => "MatchStateChanged";
        public static string ShowLeaderboard => "ShowLeaderboard";
    }
}