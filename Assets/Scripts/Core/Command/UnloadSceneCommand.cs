using PG.Core.Installer;
using Zenject;

namespace PG.Core.Command
{
    public class UnloadSceneCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        protected override void ExecuteInternal(Signal signal)
        {
            UnloadSceneSignal loadParams = signal as UnloadSceneSignal;
            
            _sceneLoader.UnloadScene (loadParams.Scene).Done (
                () =>
                {
                    loadParams.OnComplete?.Resolve();
                },
                exception =>
                {
                    loadParams.OnComplete?.Reject(exception);
                }
            );
        }
    }
}
