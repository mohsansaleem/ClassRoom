using System.Linq;
using PG.CastleBuilder.Model.Data;

namespace PG.CastleBuilder.Model
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

