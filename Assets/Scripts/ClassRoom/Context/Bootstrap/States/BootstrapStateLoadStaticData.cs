using System.Threading.Tasks;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
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
                    .Then(async () =>
                    {
                        // For progress animation effect
                        await Task.Delay(200);
                        BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.LoadUserData);
                    })
                    .Done(() => Debug.Log("Static Data loaded."),
                        e =>
                        {
                            BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.CreateMetaData);
                        });
            }
        }
    }
}