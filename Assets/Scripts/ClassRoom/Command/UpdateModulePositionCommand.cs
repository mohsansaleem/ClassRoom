using Zenject;
using System;
using PG.ClassRoom.Context.Gameplay;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class UpdateModulePositionCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private GamePlayContextModel _gamePlayContextModel;

        protected override void ExecuteInternal(Signal signal)
        {
            UpdateModulePositionSignal param = signal as UpdateModulePositionSignal;
            try
            {
                Service.UpdateModulePosition(param.Model, param.Model.CurrentPosition.Value, param.Position)
                    .Done(() =>
                    {
                        param.Model.RemoteData.CurrentPosition = param.Position;
                        param.Model.CurrentPosition.Value = param.Position;
                        param.OnComplete.Resolve();
                    }, param.OnComplete.Reject);
            }
            catch (Exception ex)
            {
                param.OnComplete.Reject(ex);
            }
        }
    }
}