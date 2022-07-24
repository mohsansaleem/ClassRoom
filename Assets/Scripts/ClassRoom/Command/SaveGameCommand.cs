using Zenject;
using UnityEngine;
using System;
using PG.ClassRoom.Model.Remote;
using PG.Core.Command;
using PG.Core.Installer;

namespace PG.ClassRoom.Command
{
    public class SaveGameCommand : RemoteCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;

        protected override void ExecuteInternal(Signal signal)
        {
            try
            {
                Service.SaveUserData(_remoteDataModel.UserData).Done((userData) => signal.OnComplete.Resolve(), signal.OnComplete.Reject);
            }
            catch(Exception ex)
            {
                Debug.LogError("Error while saving: "+ ex);
                signal.OnComplete.Reject(new Exception("Error while saving game.", ex));
            }
        }
    }

}
