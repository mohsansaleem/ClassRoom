using Zenject;
using System;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.CastleBuilder.Command
{
    public class LoadUserDataCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private readonly BootstrapModel _bootstrapModel;

        protected override void ExecuteInternal(Signal signal)
        {
            try
            {
                Service.GetUserData()
                    .Done(userDate =>
                    {
                        _remoteDataModel.SeedUserData(userDate);
                        signal.OnComplete.Resolve();
                    }, signal.OnComplete.Reject);
            }
            catch (Exception ex)
            {
                signal.OnComplete.Reject(ex);
            }
        }
    }
}