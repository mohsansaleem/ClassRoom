using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Views.Bootstrap;

namespace PG.CastleBuilder.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapState : StateBehaviour
        {
            protected readonly Bootstrap.BootstrapMediator Mediator;
            protected readonly BootstrapModel BootstrapModel;
            protected readonly BootstrapView View;

            protected readonly ProjectContextInstaller.Settings GameSettings;

            public BootstrapState(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                this.BootstrapModel = mediator._bootstrapModel;
                this.View = mediator._view;
                this.SignalBus = mediator.SignalBus;

                this.GameSettings = mediator._gameSettings;
            }
        }
    }
}
