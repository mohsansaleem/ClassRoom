using PG.ClassRoom.Command;
using PG.ClassRoom.Installer;
using PG.Core.Installer;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Context.Gameplay
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
            
            /*Container.BindFactory<X, PlaceholderFactory<X>>().FromMethod((injectionContext)=>{
                var gameObject = PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0);
                injectionContext.Inject(gameObject);
                return gameObject;
            });*/
        }
    }
}