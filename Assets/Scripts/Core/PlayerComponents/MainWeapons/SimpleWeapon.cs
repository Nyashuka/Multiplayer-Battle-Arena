using Core.PlayerComponents.MainWeapons.Abstract;
using Fusion;
using UnityEngine;

namespace Core.PlayerComponents.MainWeapons
{
    public class SimpleWeapon : Weapon
    {
        [SerializeField] private PhysxBall _prefabPhysxBall; 
        [Networked] private TickTimer delay { get; set; } 
        
        public override void Fire(Vector3 direction, NetworkRunner runner, PlayerRef owner)
        {
            if (!HasStateAuthority)
                return;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                delay = TickTimer.CreateFromSeconds(runner, 0.5f);

                Runner.Spawn(
                    _prefabPhysxBall,
                    transform.position + transform.forward, // правильна позиція спавну
                    Quaternion.identity,
                    owner,
                    (runner, o) =>
                    {
                        o.GetComponent<PhysxBall>().Init(10 * transform.forward);
                    });
            }
        }
    }
}