using System;

namespace PG.ClassRoom.Model.Data
{
    public enum EModuleType
    {
        Workshop = 0,
        Decoration
    }

    public enum ECurrencyType
    {
        None = 0,
        Gold,
        Happiness
    }
    
    [Serializable]
    public class ModuleData: GridEntityData
    {
        public uint StaticId;
        public string ModuleName;
        public EModuleType ModuleType;
        public ECurrencyType ProductionType;
        public long Production;
        public long StorageCapacity;
        public long CostGold;
        public long PlayerXP;
        public int LevelRequired;
        public string SpriteName;

        public string GetCostString()
        {
            return (CostGold > 0 ? (" Gold :" + CostGold) : "");
        }
    }
}