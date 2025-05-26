using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class PhysxBall : NetworkBehaviour
    {
        [SerializeField] private float throwForce = 10f;
        [SerializeField] private float lifeTime = 5f;
        [Networked] private TickTimer Life { get; set; }

        public void Init(Vector3 forward)
        {
            Life = TickTimer.CreateFromSeconds(Runner, lifeTime);
            GetComponent<Rigidbody>().velocity = forward * throwForce;
        }

        public override void FixedUpdateNetwork()
        {
            if(Life.Expired(Runner))
                Runner.Despawn(Object);
        }
    }
}