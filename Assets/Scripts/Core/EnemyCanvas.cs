using UnityEngine;

namespace Core
{
    public class EnemyCanvas : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;

        public void Start()
        {
            cameraTransform = Camera.main.transform;     
        }
        
        private void LateUpdate()
        {
            transform.LookAt(transform.position + cameraTransform.forward);    
        }
    }
}
