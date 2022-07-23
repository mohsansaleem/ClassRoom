using PG.Core.Installer;
using Zenject;

namespace PG.Core.Command
{
    public class LoadSceneCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        protected override void ExecuteInternal(Signal signal)
        {
            LoadSceneSignal loadParams = signal as LoadSceneSignal;
            
            _sceneLoader.LoadScene (loadParams.Scene).Done(
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
