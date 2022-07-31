using System;
using System.Collections.Generic;
using System.Linq;
using PG.ClassRoom.Command;
using PG.ClassRoom.Context.Bootstrap;
using PG.ClassRoom.Context.Gameplay;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Service;
using PG.Core.Installer;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Installer
{
    public class ProjectContextInstaller : CoreContextInstaller
    {
        [Inject] private Settings _settings;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<StaticDataModel>().AsSingle();
            Container.Bind<RemoteDataModel>().AsSingle();
            Container.Bind<RealtimeDataModel>().AsSingle();
            Container.Bind<BootstrapContextModel>().AsSingle();
            Container.Bind<GamePlayContextModel>().AsSingle();
            
            Container.DeclareSignal<SaveGameSignal>();
            Container.Bind<SaveGameCommand>().AsSingle();
            Container.BindSignal<SaveGameSignal>().ToMethod<SaveGameCommand>((x, y) => x.Execute(y)).FromResolve();

            Container.DeclareSignal<AttachModuleToPointerSignal>();
            Container.BindFactory<ModuleRemoteData, ModuleRemoteDataModel, ModuleRemoteDataModel.Factory>();
            
            Container.BindFactory<Signal, Signal.Factory>().AsSingle();

            // We can bind different Service here like Remote server.
            // For now I am using local plain files.
            Container.BindInterfacesTo<FileStorageService>().AsSingle();

            //Container.Bind<RealtimeHub>().FromNewComponentOnNewGameObject().AsSingle();
            //Container.Bind<DOTS_Hub>().AsSingle();
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
            public PlayerController PlayerPrefab;
        }   
    }
    
    [Serializable]
    public class BuildingPrefab
    {
        public uint StaticId;
        public GameObject Prefab;
    }
}