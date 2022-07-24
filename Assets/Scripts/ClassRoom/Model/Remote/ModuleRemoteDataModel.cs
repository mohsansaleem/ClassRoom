using System;
using PG.ClassRoom.Context.Gameplay;
using PG.ClassRoom.Model.Data;
using UniRx;
using Zenject;

namespace PG.ClassRoom.Model.Remote
{
    public class ModuleRemoteDataModel : EntityRemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly SignalBus _signalBus;

        public readonly ReactiveProperty<ModuleRemoteData> ModuleRemoteData;

        [Inject]
        public ModuleRemoteDataModel(ModuleRemoteData moduleRemoteData)
        {
            ModuleRemoteData = new ReactiveProperty<ModuleRemoteData>(moduleRemoteData);
        }

        public ModuleData Data => ModuleRemoteData.Value.ModuleData;
        public ModuleRemoteData RemoteData => ModuleRemoteData.Value;

        public void SeedModuleRemoteData(ModuleRemoteData moduleRemoteData)
        {
            ModuleRemoteData.Value = moduleRemoteData;
            CurrentPosition.Value = moduleRemoteData.CurrentPosition;
        }

        public void Collect()
        {
            // TODO: Fire a signal and perform in Command.
            long diff = (long)(DateTime.Now - RemoteData.LastCommandTime).TotalSeconds * RemoteData.ModuleData.Production;
            if (diff > RemoteData.ModuleData.StorageCapacity)
            {
                diff = RemoteData.ModuleData.StorageCapacity;
            }
            
            RemoteData.LastCommandTime = DateTime.Now;

            switch (Data.ProductionType)
            {
                case ECurrencyType.Gold:
                    _remoteDataModel.Gold.Value += diff;
                    _remoteDataModel.UserData.Gold += diff;
                    break;
            }
            
            _signalBus.Fire<SaveGameSignal>();
        }

        public bool CanProduce => Data.ProductionType != ECurrencyType.None && Data.ProductionType != ECurrencyType.Happiness;

        public class Factory : PlaceholderFactory<ModuleRemoteData, ModuleRemoteDataModel>
        {
            public override ModuleRemoteDataModel Create(ModuleRemoteData param)
            {
                ModuleRemoteDataModel model = base.Create(param);
                
                // Seeding the GameStateData to the in-memory Model.
                model.SeedModuleRemoteData(param);

                return model;
            }

            public override void Validate()
            {
                base.Validate();
            }
        }
    }
}