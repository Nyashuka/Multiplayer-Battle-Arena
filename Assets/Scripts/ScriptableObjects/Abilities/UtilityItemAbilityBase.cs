using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public abstract class UtilityItemAbilityBase : ScriptableObject, IUtilityItemAbility
    {
        public abstract void Use(NetworkRunner runner, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext);
    }
}