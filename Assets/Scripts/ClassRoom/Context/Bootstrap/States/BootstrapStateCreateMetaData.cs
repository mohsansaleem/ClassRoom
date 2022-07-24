using System.Threading.Tasks;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Data;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateCreateMetaData : BootstrapState
        {
            private readonly StaticDataModel _staticDataModel;

            public BootstrapStateCreateMetaData(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
                _staticDataModel = mediator._staticDataModel;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                
                MetaData MetaData = GameSettings.MetaDataAsset.Meta;

                SignalFactory.Create<CreateMetaDataSignal>().CreateMetaData(MetaData).Then(
                    async () =>
                    {
                        // For progress animation effect
                        await Task.Delay(200);
                        BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.LoadStaticData);
                    }
                    ,e =>
                    {
                        Debug.LogError("Exception Creating new Meta: " + e);
                    });
            }
        }
    }
}