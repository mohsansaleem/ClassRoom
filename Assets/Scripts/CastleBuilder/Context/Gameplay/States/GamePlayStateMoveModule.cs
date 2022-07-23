using UnityEngine;

namespace PG.CastleBuilder.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayStateMoveModule : GamePlayState
        {
            
            private Vector3 _startMousePosition;
            private Vector3 _tmpVector;
            private Vector3 _originalPosition;
            
            public GamePlayStateMoveModule(GamePlayMediator mediator) : base(mediator)
            {
                
            }
        }
    }
}