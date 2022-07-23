using System.Linq;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Model.Remote
{
    public class RemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private ModuleRemoteDataModel.Factory _moduleFactory;
        [Inject] private ProjectContextInstaller.Settings _settings;

        public UserData UserData { get; private set; }

        public readonly ReactiveProperty<string> PlayerName;
        public readonly ReactiveProperty<long> PlayerLevel;
        public readonly ReactiveProperty<float> PlayerXPProgress;
        public readonly ReactiveProperty<float> HappinessProgress;
        public readonly ReactiveProperty<Sprite> HappinessSprite;
        public readonly ReactiveProperty<long> Gold;

        public readonly ReactiveDictionary<long, ModuleRemoteDataModel> ModuleRemoteDatas;

        public RemoteDataModel()
        {
            PlayerName = new ReactiveProperty<string>();
            PlayerLevel = new ReactiveProperty<long>();
            PlayerXPProgress = new ReactiveProperty<float>();
            HappinessProgress = new ReactiveProperty<float>(0);
            HappinessSprite = new ReactiveProperty<Sprite>();
            Gold = new ReactiveProperty<long>(0);

            ModuleRemoteDatas = new ReactiveDictionary<long, ModuleRemoteDataModel>();
        }

        public void SeedUserData(UserData userData)
        {
            UserData = userData;

            foreach (var module in userData.ModulesInGrid)
            {
                AddModuleRemoteData(module);
            }

            PlayerName.Value = userData.PlayerName;
            SetPlayerXP(userData.PlayerXP, userData.PlayerLevel);
            UpdateHappiness(userData.Happiness);
            Gold.Value = userData.Gold;
        }

        public void SetPlayerXP(long xp, long level)
        {
            PlayerLevel.Value = level;
            PlayerXPProgress.Value = (float)(xp - _staticDataModel.MetaData.PlayerLevels[level - 1]) / 
                                     (_staticDataModel.MetaData.PlayerLevels[level] -
                                      _staticDataModel.MetaData.PlayerLevels[level - 1]);
        }

        public void UpdateHappiness(long happiness)
        {
            UserData.Happiness = happiness;
            HappinessProgress.Value = (float)UserData.Happiness / 100;
            // We can use multiple sprite depending on the level. For now only have one sprite.
            HappinessSprite.Value = _settings.GetSprite("Smiley");
        }

        public void AddModuleRemoteData(ModuleRemoteData module)
        {
            // Seeding the Meta to GameState Instances.
            module.GridEntityData =
                _staticDataModel.MetaData.Modules.First(a => a.StaticId.Equals(module.StaticId));
            
            // Creating Models respecting to GameStateEntries.
            var tmp = _moduleFactory.Create(module);

            // Adding it to the Modules List.
            ModuleRemoteDatas.Add(module.RemoteId, tmp);
        }
    }
}

