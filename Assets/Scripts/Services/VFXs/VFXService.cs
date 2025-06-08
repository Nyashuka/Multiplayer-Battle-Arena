using Services.ServiceLocatorModule.Abstract;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.VFXs
{
    public class VFXService : IService
    {
        public void PlayLocalVFX(ParticleSystem particleSystemPrefab, 
            Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var particle = parent ? 
                Object.Instantiate(particleSystemPrefab, position, rotation, parent) : 
                Object.Instantiate(particleSystemPrefab, position, rotation);
            
            particle.Play();
            
            Object.Destroy(particle.gameObject, particle.main.duration);
        }
    }
}