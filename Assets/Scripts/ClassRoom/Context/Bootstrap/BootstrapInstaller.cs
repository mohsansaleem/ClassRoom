using PG.ClassRoom.Command;
using PG.ClassRoom.Views.Bootstrap;
using PG.Core.Installer;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Context.Bootstrap
{
    public class BootstrapInstaller : ContextInstaller<BootstrapMediator>
    {
        [SerializeField]
        private BootstrapView _view;

        public override void InstallBindings()
        {
            Container.DeclareSignal<LoadStaticDataSignal>().RunAsync();
            Container.Bind<LoadStaticDataCommand>().AsSingle();
            Container.BindSignal<LoadStaticDataSignal>()
                .ToMethod<LoadStaticDataCommand>(cmd => cmd.Execute)
                .FromResolve();

            Container.DeclareSignal<CreateMetaDataSignal>();
            Container.Bind<CreateMetaDataCommand>().AsSingle();
            Container.BindSignal<CreateMetaDataSignal>()
                .ToMethod<CreateMetaDataCommand>(cmd => cmd.Execute)
                .FromResolve();
            
            Container.DeclareSignal<LoadUserDataSignal>();
            Container.Bind<LoadUserDataCommand>().AsSingle();
            Container.BindSignal<LoadUserDataSignal>()
                .ToMethod<LoadUserDataCommand>(cmd => cmd.Execute)
                .FromResolve();

            Container.DeclareSignal<CreateUserDataSignal>();
            Container.Bind<CreateUserDataCommand>().AsSingle();
            Container.BindSignal<CreateUserDataSignal>()
                .ToMethod<CreateUserDataCommand>(cmd => cmd.Execute)
                .FromResolve();

            Container.BindInstances(_view);
            
            base.InstallBindings();
        }
    }
}
