namespace PG.CastleBuilder.Context.Bootstrap
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
                    () =>
                    {
                        BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.GamePlay);
                    }
                    , e =>
                    {
                        BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.CreateUserData);
                    });
            }
        }
    }
}