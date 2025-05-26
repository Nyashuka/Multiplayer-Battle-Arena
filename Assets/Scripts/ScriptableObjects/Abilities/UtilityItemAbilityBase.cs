using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public abstract class UtilityItemAbilityBase : ScriptableObject, IUtilityItemAbility
    {
        public abstract void Use(UtilityItemConfig config, ItemUseContext itemUseContext);
    }
}