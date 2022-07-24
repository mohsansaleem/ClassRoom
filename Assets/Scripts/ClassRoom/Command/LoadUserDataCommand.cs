using Zenject;
using System;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class LoadUserDataCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private readonly BootstrapContextModel _bootstrapContextModel;

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