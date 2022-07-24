using PG.ClassRoom.Model.Remote;
using PG.Core.Installer;
using RSG;
using UnityEngine;

namespace PG.ClassRoom.Context.Gameplay
{
    public class SaveGameSignal : Signal { }
    
    public class UpdateModulePositionSignal : Signal
    {
        public ModuleRemoteDataModel Model;
        public Vector3 Position;

        public Promise UpdateModulePosition(ModuleRemoteDataModel moduleModel, Vector3 pos)
        {
            Model = moduleModel;
            Position = pos;
            this.Fire();

            return OnComplete;
        }
    }

    public class AddNewModuleSignal : Signal
    {
        public uint ModuleStaticId;
        public bool AutoPlace;
        public Vector3 Position;

        public Promise AddModule(uint moduleStaticId)
        {
            ModuleStaticId = moduleStaticId;
            AutoPlace = true;
            this.Fire();

            return OnComplete;
        }
        
        public Promise AddModule(uint moduleStaticId, Vector3 pos)
        {
            ModuleStaticId = moduleStaticId;
            AutoPlace = false;
            Position = pos;
            
            this.Fire();

            return OnComplete;
        }
    }
    
    public class RemoveModuleSignal : Signal
    {
        public long ModuleRemoteId;

        public Promise RemoveModule(long moduleRemoteId)
        {
            ModuleRemoteId = moduleRemoteId;
            this.Fire();

            return OnComplete;
        }
    }
}