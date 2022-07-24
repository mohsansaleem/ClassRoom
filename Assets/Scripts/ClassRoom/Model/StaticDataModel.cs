using System.Linq;
using PG.ClassRoom.Model.Data;

namespace PG.ClassRoom.Model
{
    public class StaticDataModel
    {
        public MetaData MetaData;

        public ModuleData GetModuleData(uint moduleStaticId)
        {
            return MetaData.Modules.First(m => m.StaticId.Equals(moduleStaticId));
        }
        
        public void SeedMetaData(MetaData metaData)
        {
            MetaData = metaData;
        }
    }
}

