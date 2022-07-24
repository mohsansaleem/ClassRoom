using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Gameplay;

namespace PG.ClassRoom.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayState : StateBehaviour
        {
            protected readonly GamePlayMediator Mediator;
            
            protected readonly GamePlayContextModel GamePlayContextModel;
            protected readonly GamePlayView View;

            protected readonly ProjectContextInstaller.Settings ProjectSettings;

            protected readonly RemoteDataModel RemoteDataModel;
            protected readonly StaticDataModel StaticDataModel;

            protected readonly GridController GridController;
            protected readonly CameraController CameraController;

            public GamePlayState(GamePlayMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                
                this.GamePlayContextModel = mediator._gamePlayContextModel;
                this.View = mediator._view;

                this.ProjectSettings = mediator._projectSettings;

                this.RemoteDataModel = mediator._remoteDataModel;
                this.StaticDataModel = mediator._staticDataModel;

                GridController = mediator._gridController;
                CameraController = mediator._cameraController;
            }
        }
    }
}
