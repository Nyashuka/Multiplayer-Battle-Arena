using UnityEngine;

namespace ScriptableObjects.AdditionWeapons
{
    [CreateAssetMenu(menuName = "Utility Items/Medkit Config")]
    public class MedKitItemConfig : UtilityItemConfig
    {   
        [Header("Config")]
        [SerializeField] private int healAmount;
    }
}