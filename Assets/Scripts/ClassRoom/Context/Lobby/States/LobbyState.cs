using PG.ClassRoom.Context.Lobby.Elements.Friend;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Gameplay;
using PG.Core.FSM;

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator
    {
        private class LobbyState : StateMachineMediator.StateBehaviour
        {
            protected readonly Lobby.LobbyMediator Mediator;
            
            protected readonly LobbyContextModel LobbyContextModel;
            protected readonly LobbyView View;

            protected readonly ProjectContextInstaller.Settings ProjectSettings;

            protected readonly RemoteDataModel RemoteDataModel;
            protected readonly RealtimeDataModel RealtimeDataModel;
            protected readonly StaticDataModel StaticDataModel;

            protected readonly RoomsListView RoomsListView;

            public LobbyState(Lobby.LobbyMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                
                this.LobbyContextModel = mediator._lobbyContextModel;
                this.View = mediator._view;

                this.ProjectSettings = mediator._projectSettings;

                this.RemoteDataModel = mediator._remoteDataModel;
                this.RealtimeDataModel = mediator._realtimeDataModel;
                this.StaticDataModel = mediator._staticDataModel;

                this.RoomsListView = mediator._modulesListView;
            }
        }
    }
}
