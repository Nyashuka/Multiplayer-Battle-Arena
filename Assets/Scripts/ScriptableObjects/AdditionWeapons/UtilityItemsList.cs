using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    [CreateAssetMenu(menuName = "Utility Items/Utility Items List")]
    public class UtilityItemsList : ScriptableObject
    {
        [SerializeField] private List<UtilityItemConfig> utilityItemConfigs;
        
        public IReadOnlyList<UtilityItemConfig> UtilityItemConfigs => utilityItemConfigs;
    }
}