using Fusion;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class PlayerLives : NetworkBehaviour
    {
        [Networked] public int Lives { get; private set; }

        [SerializeField] private int initialLives = 3;

        public override void Spawned() {
            if (HasStateAuthority) {
                Lives = initialLives;
            }
        }

        public bool TryConsumeLife() {
            if (Lives <= 0) return false;

            Lives--;
            return true;
        }

        public void ResetLives() {
            if (HasStateAuthority)
                Lives = initialLives;
        }

        public bool HasLives => Lives > 0; 
    }
}