using System.Collections.Generic;
using System.Linq;
using Core.MatchmakingComponents;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule;
using UnityEngine;

namespace Core.UtilityItems
{
    public class UtilityItemHandler : NetworkBehaviour
    {
        [Networked] private string EquippedItemId { get; set; }
        
        [SerializeField] private float itemCooldown;
        
        private Dictionary<string, UtilityItemConfig> _itemDatabase;
        
        [Networked] private TickTimer TimerCooldownServer { get; set; }
        private TickTimer TimerCooldownClient { get; set; }
        
        public override void Spawned()
        {
            var utilities = 
                ServiceLocator.Instance.GetService<UtilityItemsDatabaseService>().GetAll();
            _itemDatabase = utilities.ToDictionary(c => c.Id);
        }
        
        
        public void UseItem(UtilityItemUseContext utilityItemUseContext)
        {
            if (HasInputAuthority)
            {
                if (TimerCooldownClient.ExpiredOrNotRunning(Runner) && TimerCooldownServer.ExpiredOrNotRunning(Runner))
                {
                    Rpc_UseItem(utilityItemUseContext);
                    TimerCooldownClient = TickTimer.CreateFromSeconds(Runner, itemCooldown);
                }
            }
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_UseItem(UtilityItemUseContext utilityItemUseContext, RpcInfo info = default)
        {
            ServerUse(utilityItemUseContext, 
                info.Source == PlayerRef.None && Runner.IsServer ? Runner.LocalPlayer : info.Source);
        }
        
        private void ServerUse(UtilityItemUseContext utilityItemUseContext, PlayerRef owner)
        {
            if(!HasStateAuthority) return;    
            
            if(!TimerCooldownServer.ExpiredOrNotRunning(Runner)) return;
            
            TimerCooldownClient = TickTimer.CreateFromSeconds(Runner, itemCooldown);
            
            if (!_itemDatabase.TryGetValue(EquippedItemId, out var config)) return;
        
            utilityItemUseContext.Owner = owner;
        
            var playersContext = ServiceLocator.Instance.GetService<IPlayersListContext>();
            if (playersContext!= null && playersContext.Players.TryGetValue(owner, out var user))
            {
                config.Ability.Use(Runner, user, config, utilityItemUseContext);
            } 
        }
        
        public void SetItem(string id)
        {
            if(!HasStateAuthority) return;
        
            EquippedItemId = id;
            Rpc_LocalSetupItem();
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_LocalSetupItem()
        {
            if (Runner.LocalPlayer == Object.InputAuthority)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.UtilityItemReceived, 
                    new UtilityItemReceivedEventArgs(EquippedItemId), true); 
            }
        }
        
        public void Reset()
        {
            if(!HasStateAuthority) return;
        
            Rpc_ResetCooldown();
            TimerCooldownServer = default;
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
        private void Rpc_ResetCooldown()
        {
            TimerCooldownClient = default;
        }
        
    } 
}