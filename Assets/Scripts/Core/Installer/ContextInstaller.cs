using Zenject;

namespace PG.Core.Installer
{
    public abstract class ContextInstaller<T> : MonoInstaller
    {
        public override void InstallBindings()
        { 
            Container.BindFactory<Signal, Signal.Factory>().AsSingle();
            Container.BindInterfacesTo<T>().AsSingle();
        }
    }
}