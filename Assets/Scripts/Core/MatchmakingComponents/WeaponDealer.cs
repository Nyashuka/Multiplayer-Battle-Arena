using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Core.PlayerComponents;
using Fusion;
using Infrastructure.Factories;
using ScriptableObjects.AdditionWeapons;
using ScriptableObjects.Weapons;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocator;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class WeaponDealer : NetworkBehaviour
    {
        [SerializeField] private UtilityItemsList utilityItemsList;

        private Dictionary<PlayerRef, Player> _players = new();
        private WeaponDatabaseService _weaponDatabase;

        public void Initialize(Dictionary<PlayerRef, Player> players)
        {
            _players = players;
        }

        public override void Spawned()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.WeaponRequested, OnWeaponRequested);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.UtilityItemRequested, OnUtilityItemsRequested);
            _weaponDatabase = ServiceLocator.Instance.GetService<WeaponDatabaseService>();
        }

        private void OnUtilityItemsRequested(IEventBusArgs args)
        {
            if (args is UtilityItemRequestedEventArgs utilityItemRequestedEventArgs)
            {
                Rpc_RequestUtilityItem(utilityItemRequestedEventArgs.UtilityItemId);
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_RequestUtilityItem(string utilityItemId, RpcInfo info = default )
        {
            if(!HasStateAuthority) return;

            var source = info.Source == PlayerRef.None ? Runner.LocalPlayer : info.Source;

            if (!_players.TryGetValue(source, out Player player)) return;
            
            var utilityItemConfig = utilityItemsList.UtilityItemConfigs
                .FirstOrDefault(x => x.Id == utilityItemId);

            if (utilityItemConfig == null) return;
            
            player.SetUtilityItem(utilityItemConfig.Id);
            Debug.Log("Got " + utilityItemId);
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

            var source = info.Source == PlayerRef.None ? Runner.LocalPlayer : info.Source;

            if (!_players.TryGetValue(source, out Player player)) return;
            
            var weaponConfig = _weaponDatabase.GetById(weaponId);

            if (weaponConfig == null) return;
            
            var weaponFactory = new MainWeaponFactory(Runner, weaponConfig);
            var weapon = weaponFactory.Create(source, player.MainWeaponTransform);
                    
            player.SetWeapon(weapon);
            Debug.Log("Success got " + weaponId);
        }
    }
}