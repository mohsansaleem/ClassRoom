using System;
using PG.ClassRoom.Model;
using UnityEngine;
using Zenject;
using PG.Core.Command;
using PG.Core.Installer;
using RSG;

namespace PG.ClassRoom.Command
{
    public class LoadStaticDataCommand : RemoteCommand
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        protected override void ExecuteInternal(Signal signal)
        {
            var sequence = Promise.Sequence(
                () => LoadMetaJson()
                // Add other Jsons or asset bundles etc.
            );

            sequence
                .Then(() =>
                    {
                        Debug.Log(string.Format("{0} , static data load completed!", this));
                        signal.OnComplete.Resolve();
                    }, 
                    exception => signal.OnComplete.Reject(new Exception("Static Data Loading Failed"))); 
        }

        // For now just loading everything from StreamingAssets. Proper way would be loading it from AssetBudles.
        private IPromise LoadMetaJson()
        {
            Promise promiseReturn = new Promise();

            try
            {
                Service.GetMetaData()
                    .Then(_staticDataModel.SeedMetaData)
                    .Done(metaData => promiseReturn.Resolve(), promiseReturn.Reject);
            }
            catch(Exception ex)
            {
                promiseReturn.Reject(ex);
            }

            return promiseReturn;
        }
    }
}