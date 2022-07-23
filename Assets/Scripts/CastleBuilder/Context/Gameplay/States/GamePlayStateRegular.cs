using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;

namespace PG.CastleBuilder.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayStateRegular : GamePlayState
        {
            public GamePlayStateRegular(GamePlayMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.LowerPanel.SetActive(true);
                
                Mediator.OnClickAtModule += OnClickAtModule;
            }

            private void OnClickAtModule(ModuleRemoteDataModel model)
            {
                GamePlayModel.SelectedModule.Value = model;
                GamePlayModel.ChangeState(EGamePlayState.ModuleMenu);
            }

            public override void OnStateExit()
            {
                base.OnStateExit();
                
                View.LowerPanel.SetActive(false);
                
                Mediator.OnClickAtModule -= OnClickAtModule;
            }
        }
    }
}