using System.Threading.Tasks;
using PG.Core.Installer;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateGamePlay : BootstrapState
        {
            public BootstrapStateGamePlay(Bootstrap.BootstrapMediator mediator):base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                
                SignalFactory.Create<LoadUnloadScenesSignal>().Load(new[] { Scenes.GamePlay }).Done
                (async () =>
                    {
                        // For progress animation effect
                        await Task.Delay(500);
                        View.Hide();
                        Debug.Log("Loading Finished.");
                    }
                );
            }
        }
    }
}