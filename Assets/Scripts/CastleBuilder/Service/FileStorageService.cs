using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Model.Response;
using RSG;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Service
{
    public class FileStorageService : IRemoteDataService
    {
        // Needed these to have the data to Authorize. 
        // For Server state it will be authorized on Server.
        [Inject] private StaticDataModel _staticDataModel;

        private UserData _userData;
        private UserData UserData
        {
            get
            {
                if (_userData == null)
                {
                    if (!TryGetUserData(out _userData))
                    {
                        Debug.LogError("Something went wrong. Unable to get the UserData.");
                    }
                }

                return _userData;
            }
        }

        public IPromise<MetaData> CreateMetaData(MetaData metaData)
        {
            Promise<MetaData> promiseReturn = new Promise<MetaData>();

            try
            {
                // TODO: MS: Encrypt the Data. For now saving plain to read and change.
                using (var writer = new StreamWriter(Constants.MetaDataFile))
                {
                    writer.Write(JsonConvert.SerializeObject(metaData, Formatting.Indented));
                    writer.Flush();
                }

                promiseReturn.Resolve(metaData);
            }
            catch (Exception ex)
            {
                promiseReturn.Reject(ex);
            }

            return promiseReturn;
        }

        public IPromise<MetaData> GetMetaData()
        {
            Promise<MetaData> promiseReturn = new Promise<MetaData>();

            try
            {
                if (!Utils.VerifyDataFolder())
                {
                    promiseReturn.Reject(new Exception("Data folder doesn't exit"));
                }
                
                // TODO: MS: Encrypt the Data. For now saving plain to read and change.
                string path = Constants.MetaDataFile;
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        MetaData metaData = JsonConvert.DeserializeObject<MetaData>(reader.ReadToEnd());
                        promiseReturn.Resolve(metaData);
                    }
                }
                else
                {
                    promiseReturn.Reject(new FileNotFoundException("MetaData File not found."));
                }
            }
            catch (Exception ex)
            {
                promiseReturn.Reject(ex);
            }

            return promiseReturn;
        }

        public IPromise<UserData> SaveUserData(UserData userData)
        {
            var promise = new Promise<UserData>();

            try
            {
                // TODO: MS: Encrypt the Data. For now saving plain to read and change.
                using (var writer = new StreamWriter(Constants.GameStateFile))
                {
                    writer.Write(JsonConvert.SerializeObject(userData, Formatting.Indented));
                }

                promise.Resolve(userData);
            }
            catch (Exception e)
            {
                promise.Reject(e);
            }

            return promise;
        }

        private bool TryGetUserData(out UserData userData)
        {
            // TODO: MS: Encrypt the Data. For now saving plain to read and change.
            string path = Constants.GameStateFile;

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    userData = JsonConvert.DeserializeObject<UserData>(reader.ReadToEnd());
                    return true;
                }
            }

            userData = null;

            return false;
        }
        
        public IPromise<UserData> GetUserData()
        {
            Promise<UserData> promise = new Promise<UserData>();

            try
            {
                if (TryGetUserData(out UserData userData))
                {
                    promise.Resolve(userData);
                }
                else
                {
                    promise.Reject(new FileNotFoundException("GameState File not found."));
                }
            }
            catch (Exception e)
            {
                promise.Reject(e);
            }

            return promise;
        }

        public IPromise<AddNewModuleResponse> AddNewModule(uint moduleStaticId, Vector3 position, bool autoPlace)
        {
            Promise<AddNewModuleResponse> promise = new Promise<AddNewModuleResponse>();

            try
            {
                ModuleData moduleData = _staticDataModel.GetModuleData(moduleStaticId);

                UserData userData = UserData;
                if (moduleData.CostGold > userData.Gold)
                {
                    promise.Reject(new Exception("Not Enough Resources."));
                    return promise;
                }

                Vector3 pos = Vector3.zero;

                if (!autoPlace)
                {
                    pos = position;
                    var gridPos = pos / Constants.GridTileSize;
                    Utils.MarkTheGrid(ref userData.Grid, (int)gridPos.x, (int)gridPos.z, moduleData.Width, moduleData.Length);
                }
                
                if (!autoPlace || OccupySpaceForModule(moduleData, out pos))
                {
                    ModuleRemoteData moduleRemoteData = new ModuleRemoteData()
                    {
                        RemoteId = DateTime.Now.Ticks,
                        StaticId = moduleData.StaticId,
                        LastCommandTime = DateTime.Now,
                        CurrentPosition = pos
                    };
                
                    userData.ModulesInGrid.Add(moduleRemoteData);
                    
                    // Updating the Data in UserData.
                    userData.Gold -= moduleData.CostGold;
                    userData.PlayerXP += moduleData.PlayerXP;
                    while (userData.PlayerLevel < _staticDataModel.MetaData.PlayerLevels.Length - 1 &&
                           userData.PlayerXP >= _staticDataModel.MetaData.PlayerLevels[userData.PlayerLevel])
                    {
                        userData.PlayerLevel++;
                    }

                    // Adding the happiness on addition. 
                    if (moduleData.ModuleType == EModuleType.Decoration && moduleData.ProductionType == ECurrencyType.Happiness)
                    {
                        userData.Happiness += moduleData.Production;
                    }

                    var response = new AddNewModuleResponse()
                    {
                        UpdatedGold = userData.Gold,
                        UpdatedXp = userData.PlayerXP,
                        UpdatedLevel = userData.PlayerLevel,
                        UpdatedHappiness = userData.Happiness,
                        ModuleRemoteData = moduleRemoteData
                    };

                    SaveUserData(userData).Done(_ => promise.Resolve(response),  promise.Reject);
                }
                else
                {
                    promise.Reject(new Exception("No Space for new Module."));
                }
            }
            catch (Exception e)
            {
                promise.Reject(e);
            }

            return promise;
        }

        public IPromise RemoveModule(long moduleRemoteId)
        {
            var promise = new Promise();

            var model = UserData.ModulesInGrid.FirstOrDefault(module => module.RemoteId.Equals(moduleRemoteId));

            if (model == null)
            {
                promise.Reject(new Exception($"Unable to find the Module with RemoteId: {moduleRemoteId}"));
            }
            else
            {
                _userData.ModulesInGrid.Remove(model);

                var data = _staticDataModel.MetaData.Modules.Find(md => md.StaticId.Equals(model.StaticId));
                // Unmark the Grid.
                var gridPos = model.CurrentPosition / Constants.GridTileSize;
                Utils.MarkTheGrid(ref _userData.Grid, (int)gridPos.x, (int)gridPos.z, data.Width, data.Length, false);
                
                SaveUserData(_userData).Done(_ => promise.Resolve(), promise.Reject);
            }
            
            return promise;
        }

        public IPromise UpdateModulePosition(ModuleRemoteDataModel model, Vector3 old, Vector3 pos)
        {
            Promise promise = new Promise();
            
            try
            {
                var grid = UserData.Grid;
                
                int posi = (int)pos.x / Constants.GridTileSize;
                int posy = (int)pos.z / Constants.GridTileSize;

                if (posi < 0 || posy < 0 || posi + model.Data.Width > grid.GetLength(0) || posy + model.Data.Length > grid.GetLength(1))
                {
                    promise.Reject(new Exception("Out of range"));
                    return promise;
                }
                
                posi = (int)old.x / Constants.GridTileSize;
                posy = (int)old.z / Constants.GridTileSize;

                for (int i = posi; i < posi + model.Data.Width; i++)
                {
                    for (int r = posy; r < posy + model.Data.Length; r++)
                    {
                        grid[i, r] = false;
                    }
                }
                
                posi = (int)pos.x / Constants.GridTileSize;
                posy = (int)pos.z / Constants.GridTileSize;
                
                for (int i = posi; i > 0 && i < posi + model.Data.Width; i++)
                {
                    for (int r = posy; r > 0 && r < posy + model.Data.Length; r++)
                    {
                        grid[i, r] = true;
                    }
                }

                SaveUserData(UserData).Done(_ => promise.Resolve(),  promise.Reject);
            }
            catch (Exception e)
            {
                promise.Reject(e);
            }

            return promise;
        }
        
        public bool OccupySpaceForModule(ModuleData moduleData, out Vector3 pos)
        {   
            bool[,] grid = UserData.Grid;
            for (int wi = 0; wi < grid.GetLength(0); wi++)
            {
                for (int li = 0; li < grid.GetLength(1); li++)
                {
                    if (IsEmptyForModule(wi, li, moduleData.Width, moduleData.Length))
                    {
                        pos = new Vector3(wi * Constants.GridTileSize, 0, li * Constants.GridTileSize);
                        
                        Utils.MarkTheGrid(ref grid, wi, li, moduleData.Width, moduleData.Length);
                        
                        return true;
                    }
                }
            }
            
            pos = Vector3.zero;
            
            return false;
        }
        
        public bool IsEmptyForModule(int wi, int li, int wm, int lm)
        {
            if (wi + wm > _staticDataModel.MetaData.GridWidth || li + lm > _staticDataModel.MetaData.GridHeight)
            {
                return false;
            }
            
            for (int w = wi; w < wi + wm; w++)
            {
                for (int l = li; l < li + lm; l++)
                {
                    if (UserData.Grid[w, l])
                        return false;
                }
            }

            return true;
        }
    }
}