using PG.Core.Installer;
using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
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
                (
                    () =>
                    {
                        View.Hide();
                        Debug.Log("Loading Finished.");
                    }
                );
            }
        }
    }
}