using UnityEngine;

namespace Core.Projectiles
{
    public class ProjectilesLauncher : MonoBehaviour
    {
        [SerializeField] private DummyProjectile projectilePrefab;
        
        public void Launch(Vector3 positionFrom, Vector3 direction, Quaternion rotation)
        {
            DummyProjectile projectile = Instantiate(projectilePrefab, positionFrom, rotation);
            projectile.Launch();
        }
    }
}