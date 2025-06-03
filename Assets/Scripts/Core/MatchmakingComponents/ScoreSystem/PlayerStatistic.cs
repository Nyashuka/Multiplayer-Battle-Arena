namespace Core.MatchmakingComponents.ScoreSystem
{
    public class PlayerStatistic
    {
        public int Kills { get; private set; }
        public int Deaths { get; private set; }
                
        public PlayerStatistic(int kills, int deaths)
        {
            Kills = kills;
            Deaths = deaths;
        }

        public void AddKill()
        {
            Kills++;
        }

        public void AddDeath()
        {
            Deaths++;
        }
    }
}