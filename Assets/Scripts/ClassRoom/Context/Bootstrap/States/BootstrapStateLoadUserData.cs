using System.Threading.Tasks;

namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateLoadUserData : BootstrapState
        {
            public BootstrapStateLoadUserData(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                
                SignalFactory.Create<LoadUserDataSignal>().LoadUserData().Then(
                    async () =>
                    {
                        // For progress animation effect
                        await Task.Delay(200);
                        BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.RealTimeServer);
                    }
                    , e =>
                    {
                        BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.CreateUserData);
                    });
            }
        }
    }
}