using PG.ClassRoom.Installer;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Views.Bootstrap;

namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapState : StateBehaviour
        {
            protected readonly Bootstrap.BootstrapMediator Mediator;
            protected readonly BootstrapContextModel BootstrapContextModel;
            protected readonly BootstrapView View;

            protected readonly ProjectContextInstaller.Settings GameSettings;

            public BootstrapState(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                this.BootstrapContextModel = mediator._bootstrapContextModel;
                this.View = mediator._view;
                this.SignalBus = mediator.SignalBus;

                this.GameSettings = mediator._gameSettings;
            }
        }
    }
}
