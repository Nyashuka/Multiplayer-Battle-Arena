using UnityEngine;

namespace VFX
{
    public class SimpleExplosiveEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem explosiveParticleSystem;

        public void Play()
        {
            explosiveParticleSystem.Play();
        }
    }
}
