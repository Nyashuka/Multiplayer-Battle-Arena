using Fusion;

namespace Core.MatchmakingComponents
{
    public class MatchTimer : NetworkBehaviour
    {
        [Networked] public float TimeRemaining { get; set; }

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                TimeRemaining = 120;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;

            TimeRemaining -= Runner.DeltaTime;
        }
    }
}