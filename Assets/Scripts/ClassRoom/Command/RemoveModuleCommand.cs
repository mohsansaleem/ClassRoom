﻿using Zenject;
using System;
using PG.ClassRoom.Context.Gameplay;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class RemoveModuleCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private StaticDataModel _staticDataModel;
        [Inject] private GamePlayContextModel _gamePlayContextModel;

        protected override void ExecuteInternal(Signal signal)
        {
            var param = signal as RemoveModuleSignal;
            try
            {
                Service.RemoveModule(param.ModuleRemoteId)
                    .Catch(param.OnComplete.Reject)
                    .Done(() =>
                    {
                        // Removing the Module from Remode Data Model.
                        if (_remoteDataModel.ModuleRemoteDatas.ContainsKey(param.ModuleRemoteId))
                        {
                            try
                            {
                                var moduleRemoteData = _remoteDataModel.UserData.ModulesInGrid.Find(m => m.RemoteId.Equals(param.ModuleRemoteId));
                                
                                // Unmark the Grid.
                                var gridPos = moduleRemoteData.CurrentPosition / Constants.GridTileSize;
                                Utils.MarkTheGrid(ref _remoteDataModel.UserData.Grid, (int)gridPos.x, (int)gridPos.z, moduleRemoteData.Width, moduleRemoteData.Length, false);

                                // Removing from the RemoteModel and RemoteData
                                _remoteDataModel.UserData.ModulesInGrid.Remove(moduleRemoteData);
                                _remoteDataModel.ModuleRemoteDatas.Remove(param.ModuleRemoteId);
                                
                                param.OnComplete.Resolve();
                            }
                            catch (Exception e)
                            {
                                param.OnComplete.Reject(e);
                            }
                        }
                    }, param.OnComplete.Reject);
            }
            catch (Exception ex)
            {
                param.OnComplete.Reject(ex);
            }
        }
    }
}