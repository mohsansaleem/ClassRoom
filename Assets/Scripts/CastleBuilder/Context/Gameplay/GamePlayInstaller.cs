using PG.CastleBuilder.Command;
using PG.CastleBuilder.Installer;
using PG.Core.Installer;
using Zenject;

namespace PG.CastleBuilder.Context.Gameplay
{
    public class GamePlayInstaller : ContextInstaller<GamePlayMediator>
    {
        [Inject] private ProjectContextInstaller.Settings _settings;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.DeclareSignal<UpdateModulePositionSignal>();
            Container.Bind<UpdateModulePositionCommand>().AsSingle();
            Container.BindSignal<UpdateModulePositionSignal>()
                .ToMethod<UpdateModulePositionCommand>(command => command.Execute)
                .FromResolve();

            Container.DeclareSignal<AddNewModuleSignal>();
            Container.Bind<AddNewModuleCommand>().AsSingle();
            Container.BindSignal<AddNewModuleSignal>()
                .ToMethod<AddNewModuleCommand>(command => command.Execute)
                .FromResolve();
            
            Container.DeclareSignal<RemoveModuleSignal>();
            Container.Bind<RemoveModuleCommand>().AsSingle();
            Container.BindSignal<RemoveModuleSignal>()
                .ToMethod<RemoveModuleCommand>(command => command.Execute)
                .FromResolve();
        }
    }
}