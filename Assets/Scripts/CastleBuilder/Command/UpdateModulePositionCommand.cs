using Zenject;
using System;
using PG.CastleBuilder.Context.Gameplay;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.CastleBuilder.Command
{
    public class UpdateModulePositionCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private GamePlayModel _gamePlayModel;

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