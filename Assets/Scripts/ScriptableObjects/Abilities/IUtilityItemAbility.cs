using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public interface IUtilityItemAbility
    {
        public void Use(UtilityItemConfig config, ItemUseContext itemUseContext);
    }
}