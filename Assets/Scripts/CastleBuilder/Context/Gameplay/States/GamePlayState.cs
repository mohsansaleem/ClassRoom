using PG.CastleBuilder.DOTS;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Views.Gameplay;

namespace PG.CastleBuilder.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayState : StateBehaviour
        {
            protected readonly GamePlayMediator Mediator;
            
            protected readonly GamePlayModel GamePlayModel;
            protected readonly GamePlayView View;

            protected readonly ProjectContextInstaller.Settings ProjectSettings;

            protected readonly RemoteDataModel RemoteDataModel;
            protected readonly StaticDataModel StaticDataModel;

            protected readonly DOTS_Hub DOTS_Hub;

            protected readonly GridController GridController;
            protected readonly CameraController CameraController;

            public GamePlayState(GamePlayMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                
                this.GamePlayModel = mediator._gamePlayModel;
                this.View = mediator._view;

                this.ProjectSettings = mediator._projectSettings;

                this.RemoteDataModel = mediator._remoteDataModel;
                this.StaticDataModel = mediator._staticDataModel;

                this.DOTS_Hub = mediator._dotsHub;

                GridController = mediator._gridController;
                CameraController = mediator._cameraController;
            }
        }
    }
}
