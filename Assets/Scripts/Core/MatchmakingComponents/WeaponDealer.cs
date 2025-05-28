using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Fusion;
using Infrastructure.Factories;
using ScriptableObjects.Weapons;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class WeaponDealer : NetworkBehaviour
    {
        [SerializeField] private WeaponList weaponList;

        private Dictionary<PlayerRef, Player> _players = new();

        public void Initialize(Dictionary<PlayerRef, Player> players)
        {
            _players = players;
        }

        public override void Spawned()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.WeaponRequested, OnWeaponRequested);
        }

        private void OnWeaponRequested(IEventBusArgs args)
        {
            if (args is WeaponRequestedEventArgs weaponArgs)
            {
                Debug.Log("Rpc for getting " + weaponArgs.WeaponId);
                Rpc_RequestWeapon(weaponArgs.WeaponId);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_RequestWeapon(string weaponId, RpcInfo info = default )
        {
            if(!HasStateAuthority) return;

            Debug.Log("Try getting " + weaponId);
            if (!_players.TryGetValue(info.Source, out Player player)) return;
            var weaponConfig = weaponList.Weapons.FirstOrDefault(x => x.ID == weaponId);

            if (weaponConfig == null) return;
            
            var weaponFactory = new MainWeaponFactory(Runner, weaponConfig);
            var weapon = weaponFactory.Create(info.Source, player.GetPrimaryWeaponTransform());
                    
            player.SetWeapon(weapon);
            Debug.Log("Success got " + weaponId);
        }

        public IReadOnlyList<WeaponConfigBase> GetWeapons()
        {
            return weaponList.Weapons;
        }
    }
}