using PG.ClassRoom.Model.Context;
using Photon.Pun;
using UnityEngine;

namespace PG.ClassRoom.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayStateLoad : GamePlayState
        {
            public GamePlayStateLoad(GamePlayMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                //GridController.GridSize = new Vector2(StaticDataModel.MetaData.GridWidth, StaticDataModel.MetaData.GridHeight);
                //CameraController.CurrentZoom = StaticDataModel.MetaData.MinZoomLevel;

                PlayerController playerController = Mediator._playerControllerFactory.Create();
                playerController.transform.parent = View.Environment;
                
                Mediator._gamePlayContextModel.ChangeStateAndNotify(EGamePlayState.Regular);
            }
        }
    }
}