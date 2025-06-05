using System.Collections.Generic;
using Core.PlayerComponents;
using Fusion;
using Infrastructure.Factories;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class WeaponDealer : NetworkBehaviour
    {
        private Dictionary<PlayerRef, Player> _players = new();
        private WeaponDatabaseService _weaponDatabase;
        private UtilityItemsDatabaseService _utilityDatabase;

        public void Initialize(Dictionary<PlayerRef, Player> players)
        {
            _players = players;
        }

        public override void Spawned()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.WeaponRequested, OnWeaponRequested);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.UtilityItemRequested, OnUtilityItemsRequested);
            _weaponDatabase = ServiceLocator.Instance.GetService<WeaponDatabaseService>();
            _utilityDatabase = ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.WeaponRequested, OnWeaponRequested);
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.UtilityItemRequested, OnUtilityItemsRequested);
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
            
            var utilityItemConfig = _utilityDatabase.GetById(utilityItemId);

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