using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;

namespace PG.ClassRoom.Context.Gameplay
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
                
                //Mediator.OnClickAtModule += OnClickAtModule;
            }

            private void OnClickAtModule(ModuleRemoteDataModel model)
            {
                GamePlayContextModel.SelectedModule.Value = model;
                GamePlayContextModel.ChangeState(EGamePlayState.ModuleMenu);
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