using Core.PlayerComponents;
using Core.UtilityItems.Abstract;
using Data;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace Core.UtilityItems
{
    public class Grenade : UtilityItem
    {
        private GrenadeItemConfig _config;
        private float _timer;

        public void Initialize(GrenadeItemConfig grenadeConfig)
        {
            _config = grenadeConfig;
            _timer = _config.ExplodeDelay;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority || _timer <= 0f)
                return;
            
            _timer -= Runner.DeltaTime;
            if (_timer <= 0f)
            {
                Explode(); 
            } 
        } 
        private void Explode() 
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _config.Range);
            foreach (var hit in hits)
            {
                IDamagable damagable = null;
                if (hit.TryGetComponent(out IDamagable directHit))
                {
                    damagable = directHit;
                }
                else if (hit.transform.root.TryGetComponent(out IDamagable rootHit))
                {
                    damagable = rootHit;
                }

                if (damagable != null)
                {
                    var damageData = new DamageData()
                    {
                        Attacker = Object.InputAuthority,
                        Damage = _config.Damage
                    };
                    Debug.Log(Object.InputAuthority);
                    damagable.TakeDamage(damageData);
                }
            }

            Runner.Despawn(Object);
        }
    }
}