using PG.Core.Installer;
using Zenject;

namespace PG.Core.Command
{
    public abstract class BaseCommand
    {
        [Inject]
        protected SignalBus SignalBus;
        
        public void Execute(Signal signal)
        {
            ExecuteInternal(signal);
        }

        protected abstract void ExecuteInternal(Signal signal);
    }
}