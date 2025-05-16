using UnityEngine;

namespace VFX
{
    public class SimpleExplosiveEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;

        public void Play()
        {
            particleSystem.Play();
        
            Destroy(gameObject, 5f);
        }
    }
}
