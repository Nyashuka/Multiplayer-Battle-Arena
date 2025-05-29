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
        
        public string Id => id;
        public UtilityItemAbilityBase Ability => ability;
        public Sprite Icon => icon;
    }
}