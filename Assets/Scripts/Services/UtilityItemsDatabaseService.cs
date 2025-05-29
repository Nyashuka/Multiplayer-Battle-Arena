using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.AdditionWeapons;
using Services.ServiceLocator.Abstract;

namespace Services
{
    public class UtilityItemsDatabaseService : IService
    {
        private readonly UtilityItemsList _utilityItemsList;
        private readonly Dictionary<string, UtilityItemConfig> _utilityItems;

        public UtilityItemsDatabaseService(UtilityItemsList utilityItemsList)
        {
            _utilityItemsList = utilityItemsList;
            _utilityItems = utilityItemsList.UtilityItemConfigs.ToDictionary(x => x.Id);
        }

        public IReadOnlyList<UtilityItemConfig> GetAll()
        {
            return _utilityItemsList.UtilityItemConfigs;
        }

        public UtilityItemConfig GetById(string id)
        {
            return _utilityItems[id];
        } 
    }
}