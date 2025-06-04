using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Weapons;
using Services.ServiceLocatorModule.Abstract;

namespace Services
{
    public class WeaponDatabaseService : IService
    {
        private readonly WeaponList _weaponList;
        private readonly Dictionary<string, WeaponConfigBase> _weapons;

        public WeaponDatabaseService(WeaponList weaponList)
        {
            _weaponList = weaponList;
            _weapons = _weaponList.Weapons.ToDictionary(x => x.ID);
        }

        public IReadOnlyList<WeaponConfigBase> GetAll()
        {
            return _weaponList.Weapons;
        }

        public WeaponConfigBase GetById(string id)
        {
            return _weapons[id];
        }
    }
}