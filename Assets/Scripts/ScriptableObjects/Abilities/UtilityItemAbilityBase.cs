using Core.PlayerComponents;
using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public abstract class UtilityItemAbilityBase : ScriptableObject
    {
        public abstract void Use(NetworkRunner runner, Player user, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext);
    }
}