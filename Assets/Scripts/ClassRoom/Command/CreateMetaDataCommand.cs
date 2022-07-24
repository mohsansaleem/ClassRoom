using Zenject;
using System;
using PG.ClassRoom.Context.Bootstrap;
using PG.ClassRoom.Model;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class CreateMetaDataCommand : RemoteCommand
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        protected override void ExecuteInternal(Signal signal)
        {
            CreateMetaDataSignal commandParams = signal as CreateMetaDataSignal;
            try
            {
                Service.CreateMetaData(commandParams.MetaData)
                    .Done((meta) =>
                    {
                        commandParams.OnComplete.Resolve();
                    }, commandParams.OnComplete.Reject);
            }
            catch (Exception ex)
            {
                commandParams.OnComplete.Reject(ex);
            }
        }
    }
}