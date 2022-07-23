using Zenject;
using System;
using PG.CastleBuilder.Context.Bootstrap;
using PG.CastleBuilder.Model;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.CastleBuilder.Command
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