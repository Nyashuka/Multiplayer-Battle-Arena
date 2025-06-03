using System.Collections.Generic;
using System.Linq;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

namespace Core.UtilityItems
{
    public class UtilityItemHandler : NetworkBehaviour
    {
        [Networked] private string EquippedItemId { get; set; }
        
        [SerializeField] private List<UtilityItemConfig> itemConfigs;
        private Dictionary<string, UtilityItemConfig> _itemDatabase;

        public void Awake()
        {
            _itemDatabase = itemConfigs.ToDictionary(c => c.Id);
        }

        public void UseItem(UtilityItemUseContext utilityItemUseContext)
        {
            if (HasInputAuthority)
            {
                Rpc_UseItem(utilityItemUseContext);
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
            
            if (!_itemDatabase.TryGetValue(EquippedItemId, out var config)) return;

            utilityItemUseContext.Owner = owner;
            config.Ability.Use(Runner, config, utilityItemUseContext);
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
    }
}