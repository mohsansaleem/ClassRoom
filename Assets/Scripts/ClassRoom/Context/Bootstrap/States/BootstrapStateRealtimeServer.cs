using System.Threading.Tasks;
using PG.Core.Installer;
using UnityEngine;


namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateRealtimeServer : BootstrapState
        {
            public BootstrapStateRealtimeServer(Bootstrap.BootstrapMediator mediator):base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                
                BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.Lobby);
            }
        }
    }
}