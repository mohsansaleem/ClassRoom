using PG.CastleBuilder.Model.Context;
using UnityEngine;

namespace PG.CastleBuilder.Context.Gameplay
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

                GridController.GridSize = new Vector2(StaticDataModel.MetaData.GridWidth, StaticDataModel.MetaData.GridHeight);
                CameraController.CurrentZoom = StaticDataModel.MetaData.MinZoomLevel;
                
                Mediator._gamePlayModel.ChangeStateAndNotify(EGamePlayState.Regular);
            }
        }
    }
}