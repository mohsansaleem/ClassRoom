using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateLoadStaticData : BootstrapState
        {
            public BootstrapStateLoadStaticData(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.Show();
                
                SignalFactory.Create<LoadStaticDataSignal>().LoadStaticData()
                    .Then(() =>
                    {
                        BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.LoadUserData);
                    })
                    .Done(() => Debug.Log("Static Data loaded."),
                        e =>
                        {
                            BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.CreateMetaData);
                        });
            }
        }
    }
}