using System;
using Newtonsoft.Json;

namespace PG.ClassRoom.Model.Data
{
    [Serializable]
    public class ModuleRemoteData : GridEntityRemoteData
    {
        public long RemoteId;
        public uint StaticId;
        public DateTime LastCommandTime;
        
        [JsonIgnore]
        public ModuleData ModuleData => this.GridEntityData as ModuleData;
    }
}