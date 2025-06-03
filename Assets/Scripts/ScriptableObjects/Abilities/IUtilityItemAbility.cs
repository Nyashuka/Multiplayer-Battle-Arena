using Data;
using Fusion;
using ScriptableObjects.AdditionWeapons;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public interface IUtilityItemAbility
    {
        public void Use(NetworkRunner runner, UtilityItemConfig config, UtilityItemUseContext utilityItemUseContext);
    }
}