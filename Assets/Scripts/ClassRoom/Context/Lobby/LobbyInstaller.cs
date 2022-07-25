using PG.ClassRoom.Installer;
using PG.ClassRoom.Context.Lobby.Elements.Friend;
using PG.ClassRoom.Model.Context;
using PG.Core.Installer;
using Zenject;

namespace PG.ClassRoom.Context.Lobby
{
    public class LobbyInstaller : ContextInstaller<LobbyMediator>
    {
        public RoomsListView RoomsListView;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<LobbyContextModel>().AsSingle();
            Container.Bind<RoomsListView>().FromInstance(RoomsListView).AsSingle();
        }
    }
}