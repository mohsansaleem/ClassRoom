using Zenject;
using System;
using PG.ClassRoom.Context.Gameplay;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class AddNewModuleCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private StaticDataModel _staticDataModel;
        [Inject] private GamePlayContextModel _gamePlayContextModel;

        protected override void ExecuteInternal(Signal signal)
        {
            AddNewModuleSignal param = signal as AddNewModuleSignal;
            try
            {
                if (!param.AutoPlace)
                {
                    param.Position.x = ((int) param.Position.x / Constants.GridTileSize) * Constants.GridTileSize;
                    param.Position.y = 0;
                    param.Position.z = ((int) param.Position.z / Constants.GridTileSize) * Constants.GridTileSize;
                }
                
                Service.AddNewModule(param.ModuleStaticId, param.Position, param.AutoPlace)
                    .Catch(param.OnComplete.Reject)
                    .Done((response) =>
                    {
                        // Adding the Module to the Remode Data Model.
                        _remoteDataModel.UserData.ModulesInGrid.Add(response.ModuleRemoteData);
                        _remoteDataModel.AddModuleRemoteData(response.ModuleRemoteData);

                        // Updating the Resources
                        _remoteDataModel.Gold.Value = response.UpdatedGold;
                        _remoteDataModel.SetPlayerXP(response.UpdatedXp, response.UpdatedLevel);
                        _remoteDataModel.UpdateHappiness(response.UpdatedHappiness);

                        var gridPos = response.ModuleRemoteData.CurrentPosition / Constants.GridTileSize;
                        Utils.MarkTheGrid(ref _remoteDataModel.UserData.Grid, (int)gridPos.x, (int)gridPos.z, 
                            response.ModuleRemoteData.Width, response.ModuleRemoteData.Length);
                        
                        param.OnComplete.Resolve();
                    });
            }
            catch (Exception ex)
            {
                param.OnComplete.Reject(ex);
            }
        }
    }
}