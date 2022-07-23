using System;
using System.Collections.Generic;
using System.Linq;
using PG.CastleBuilder.Command;
using PG.CastleBuilder.Context.Bootstrap;
using PG.CastleBuilder.Context.Gameplay;
using PG.CastleBuilder.DOTS;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Service;
using PG.Core.Installer;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Installer
{
    public class ProjectContextInstaller : CoreContextInstaller
    {
        [Inject] private Settings _settings;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<StaticDataModel>().AsSingle();
            Container.Bind<RemoteDataModel>().AsSingle();
            Container.Bind<BootstrapModel>().AsSingle();
            Container.Bind<GamePlayModel>().AsSingle();
            
            Container.DeclareSignal<SaveGameSignal>();
            Container.Bind<SaveGameCommand>().AsSingle();
            Container.BindSignal<SaveGameSignal>().ToMethod<SaveGameCommand>((x, y) => x.Execute(y)).FromResolve();

            Container.DeclareSignal<AttachModuleToPointerSignal>();
            Container.BindFactory<ModuleRemoteData, ModuleRemoteDataModel, ModuleRemoteDataModel.Factory>();
            
            Container.BindFactory<Signal, Signal.Factory>().AsSingle();

            // We can bind different Service here like Remote server.
            // For now I am using local plain files.
            Container.BindInterfacesTo<FileStorageService>().AsSingle();

            Container.Bind<DOTS_Hub>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            // Meta Stuff
            public DefaultGameState DefaultGameState;
            
            /// <summary>
            /// Just doing it for the ease of creating MetaData from Scriptable object.
            /// </summary>
            public DefaultMetaData MetaDataAsset;
            
            public List<BuildingPrefab> BuildingsPrefabs;
            public List<Sprite> Sprites;

            public Sprite GetSprite(string name)
            {
                return Sprites.FirstOrDefault(sprite => sprite.name == name);
            }

            public GameObject Prefab;
        }   
    }
    
    [Serializable]
    public class BuildingPrefab
    {
        public uint StaticId;
        public GameObject Prefab;
    }
}