using System.Collections.Generic;
using ScriptableObjects.Weapons;

namespace Services
{
    public class WeaponService
    {
        public static WeaponService Instance { get; } = new();
        private WeaponService() {}
        
        private WeaponList _weaponList;

        public void SetWeaponList(WeaponList weaponList)
        {
            _weaponList = weaponList;
        }

        public IReadOnlyList<WeaponConfigBase> GetAllWeapons()
        {
            return _weaponList.Weapons;
        }
    }
}