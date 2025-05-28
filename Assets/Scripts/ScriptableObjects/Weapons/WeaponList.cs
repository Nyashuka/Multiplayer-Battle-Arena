using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Weapon List")]
    public class WeaponList : ScriptableObject
    {
        [SerializeField] private List<WeaponConfigBase> weapons;
        
        public IReadOnlyList<WeaponConfigBase> Weapons => weapons;
    }
}