using System.Collections.Generic;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using UniRx;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.DOTS
{
    public class DOTS_Hub
    {
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly GamePlayModel _gamePlayModel;
        [Inject] private ProjectContextInstaller.Settings _settings;

        // Locals
        private GameObjectConversionSettings _conversionSettings;
        private EntityManager _entityManager;
        private Dictionary<uint, Entity> _entitiesPrefabs;
        private CompositeDisposable _disposables;

        private Dictionary<long, Entity> _spawnedEntities;
        private Entity _entityAttachedToPointer;

        public static DOTS_Hub Instance { private set; get; }

        public DOTS_Hub()
        {
            Instance = this;
            _entitiesPrefabs = new Dictionary<uint, Entity>();
            _disposables = new CompositeDisposable();

            _spawnedEntities = new Dictionary<long, Entity>();
        }

        public void Init()
        {
            // Create entity prefab from the game object hierarchy once
            _conversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _entitiesPrefabs.Clear();
            foreach (var buildingPrefab in _settings.BuildingsPrefabs)
            {
                _entitiesPrefabs.Add(buildingPrefab.StaticId,
                    GameObjectConversionUtility.ConvertGameObjectHierarchy(buildingPrefab.Prefab, _conversionSettings));
            }

            foreach (ModuleRemoteDataModel model in _remoteDataModel.ModuleRemoteDatas.Values)
            {
                SpawnMapEntity(model);
            }

            _remoteDataModel.ModuleRemoteDatas.ObserveAdd().Subscribe(OnModuleAdd).AddTo(_disposables);
            _remoteDataModel.ModuleRemoteDatas.ObserveRemove().Subscribe(OnModuleRemove).AddTo(_disposables);

            _gamePlayModel.ModuleToAttach.Subscribe(data =>
            {
                if (_entityManager.Exists(_entityAttachedToPointer))
                {
                    _entityManager.DestroyEntity(_entityAttachedToPointer);
                }

                if (data != null)
                {
                    AttachEntityToPointer(data);
                }
            }).AddTo(_disposables);
        }

        private void OnModuleAdd(DictionaryAddEvent<long, ModuleRemoteDataModel> evt)
        {
            SpawnMapEntity(evt.Value);
        }

        private void OnModuleRemove(DictionaryRemoveEvent<long, ModuleRemoteDataModel> evt)
        {
            if (_spawnedEntities.TryGetValue(evt.Key, out var entity))
            {
                _spawnedEntities.Remove(evt.Key);
                _entityManager.DestroyEntity(entity);
            }
            else
            {
                Debug.LogError($"Unable to find the Entity for the RemoteId: {evt.Key}");
            }
        }

        private void SpawnMapEntity(ModuleRemoteDataModel moduleRemoteData)
        {
            if (_entitiesPrefabs.TryGetValue(moduleRemoteData.RemoteData.StaticId, out Entity prefab))
            {
                // Efficiently instantiate a bunch of entities from the already converted entity prefab
                var instance = _entityManager.Instantiate(prefab);

                _spawnedEntities[moduleRemoteData.RemoteData.RemoteId] = instance;

                // Setting the Name.
                //_entityManager.SetName(instance, $"{moduleRemoteData.Data.ModuleName}: {moduleRemoteData.RemoteData.RemoteId}");
                
                // Place the instantiated entity in a grid
                _entityManager.SetComponentData(instance,
                    new Translation {Value = moduleRemoteData.RemoteData.CurrentPosition});
            }
            else
            {
                Debug.LogError(
                    $"Module with Static Id: [{moduleRemoteData.RemoteData.StaticId}] doesn't exist in the prefabs cache.");
            }
        }

        private void AttachEntityToPointer(ModuleData moduleData)
        {
            if (_entitiesPrefabs.TryGetValue(moduleData.StaticId, out Entity prefab))
            {
                // Efficiently instantiate a bunch of entities from the already converted entity prefab
                var instance = _entityManager.Instantiate(prefab);

                _entityAttachedToPointer = instance;

                // Setting the Name.
                //_entityManager.SetName(instance, $"{moduleData.ModuleName}: {moduleData.StaticId}");
                
                // Place the instantiated entity in a grid
                _entityManager.SetComponentData(instance, new Translation {Value = float3.zero});
                _entityManager.AddComponentData(instance,
                    new FollowPointerComponent()
                    {
                        PositionBuffer = new float2(moduleData.Width * Constants.GridTileSize / 2f,
                            moduleData.Length * Constants.GridTileSize / 2f)
                    });

                Utils.GetGridPosition(out Vector3 pos);
                _entityManager.AddComponentData(instance,
                    new PointerWorldPositionComponent()
                    {
                        PointerPosition = pos
                    });
            }
            else
            {
                Debug.LogError($"Module with Static Id: [{moduleData.StaticId}] doesn't exist in the prefabs cache.");
            }
        }

        public void SetPointerWorldPosition(Vector3 pos)
        {
            if (_entityManager.Exists(_entityAttachedToPointer))
            {
                _entityManager.SetComponentData(_entityAttachedToPointer, new PointerWorldPositionComponent()
                {
                    PointerPosition = pos
                });
            }
        }

        public void Cleanup()
        {
            _entitiesPrefabs.Clear();
            _spawnedEntities.Clear();

            _disposables.Dispose();
        }
    }
}