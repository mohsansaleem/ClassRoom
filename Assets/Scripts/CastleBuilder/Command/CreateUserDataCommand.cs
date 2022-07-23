using Zenject;
using System;
using PG.CastleBuilder.Context.Bootstrap;
using PG.CastleBuilder.Model;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.CastleBuilder.Command
{
    public class CreateUserDataCommand : RemoteCommand
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        protected override void ExecuteInternal(Signal signal)
        {
            CreateUserDataSignal commandParams = signal as CreateUserDataSignal;
            try
            {
                commandParams.UserData.Grid = new bool[_staticDataModel.MetaData.GridWidth, _staticDataModel.MetaData.GridHeight];
                
                Service.SaveUserData(commandParams.UserData)
                    .Done((userData) =>
                    {
                        commandParams.OnComplete.Resolve();
                    });
            }
            catch(Exception ex)
            {
                commandParams.OnComplete.Reject(ex);
            }
        }
    }
}
