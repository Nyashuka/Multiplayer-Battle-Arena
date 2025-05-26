using ScriptableObjects.Abilities;
using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    public class UtilityItemConfig : ScriptableObject
    {
        [Header("Item Params")]
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [Header("Ability")]
        [SerializeField] private UtilityItemAbilityBase ability;
        public UtilityItemAbilityBase Ability => ability; 
    }
}