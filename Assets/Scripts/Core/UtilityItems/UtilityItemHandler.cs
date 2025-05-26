using System;
using System.Collections.Generic;
using System.Linq;
using Core.PlayerComponents;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace Core.UtilityItems
{
    public class UtilityItemHandler : NetworkBehaviour
    {
        [Networked] private string EquippedItemId { get; set; }
        
        [SerializeField] private List<UtilityItemConfig> itemConfigs;
        private Dictionary<string, UtilityItemConfig> _itemDatabase;

        [SerializeField] private UtilityItemConfig defaultItem;
        
        [SerializeField] private PlayerInput input;
        
        public void Awake()
        {
            _itemDatabase = itemConfigs.ToDictionary(c => c.name);
        }

        public override void Spawned()
        {
            EquippedItemId = defaultItem.name;
        }

        public void UseItem(ItemUseContext itemUseContext)
        {
            if (HasInputAuthority)
            {
                Rpc_UseItem(itemUseContext);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_UseItem(ItemUseContext itemUseContext)
        {
            ServerUse(itemUseContext);
        }
        
        private void ServerUse(ItemUseContext itemUseContext)
        {
            if(!HasStateAuthority) return;    
            
            if (!_itemDatabase.TryGetValue(EquippedItemId, out var config)) return;

            itemUseContext.User = this;
            config.Ability.Use(config, itemUseContext);
        }
    }
}