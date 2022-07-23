using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Data;
using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
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
                    () => {
                        BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.LoadStaticData);
                    }
                    ,e =>
                    {
                        Debug.LogError("Exception Creating new Meta: " + e);
                    });
            }
        }
    }
}