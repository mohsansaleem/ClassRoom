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
                
                Mediator._realtimeHub.Connect().Then(
                    async (isSuccess) =>
                    {
                        // For progress animation effect
                        await Task.Delay(200);
                        if (isSuccess)
                            BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.Lobby);
                        else
                            Debug.LogError("photon server connection failed.");
                    }
                    , e =>
                    {
                        Debug.LogError("Unable to connect to photon");
                    });
            }
        }
    }
}