using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using UnityEngine;

namespace PG.ClassRoom.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayStatePlaceModule : GamePlayState
        {
            public GamePlayStatePlaceModule(GamePlayMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                Mediator.OnClickInsideGrid += OnClickInsideGrid;
                Mediator.OnClickOutsideGrid += OnClickOutsideGrid;
                Mediator.OnClickAtModule += OnClickAtModule;
                View.OnUpdate += SetPointerPosition;
            }

            private void SetPointerPosition()
            {
                if (GamePlayContextModel.GamePlayState.Value == EGamePlayState.PlaceModule)
                {
                    Utils.GetGridPosition(out var pos);
                }
            }
            
            private void OnClickInsideGrid(Vector3 pos)
            {
                var data = GamePlayContextModel.ModuleToAttach.Value;

                pos.x -= data.Width * Constants.GridTileSize / 2f;
                pos.z -= data.Length * Constants.GridTileSize / 2f;
                
                if (Mediator.IsValidPosition(data, pos))
                {
                    SignalFactory.Create<AddNewModuleSignal>().AddModule(data.StaticId, pos).Done(
                        DetachModuleFromPointer,
                        exception =>
                        {
                            Mediator.ShowErrorPanel($"Unable to add the {data.ModuleName}");
                            
                            DetachModuleFromPointer();
                        });
                }
            }

            private void OnClickOutsideGrid()
            {
                Mediator.ShowErrorPanel($"Trying to add outside of the Grid.");

                DetachModuleFromPointer();
            }

            private void OnClickAtModule(ModuleRemoteDataModel model)
            {
                Mediator.ShowErrorPanel($"Trying to add over previous module.");

                DetachModuleFromPointer();
            }

            private void DetachModuleFromPointer()
            {
                GamePlayContextModel.ModuleToAttach.Value = null;
                GamePlayContextModel.ChangeState(EGamePlayState.Regular);
            }

            public override void OnStateExit()
            {
                base.OnStateExit();

                Mediator.OnClickInsideGrid -= OnClickInsideGrid;
                Mediator.OnClickOutsideGrid -= OnClickOutsideGrid;
                Mediator.OnClickAtModule -= OnClickAtModule;
                View.OnUpdate -= SetPointerPosition;
            }
        }
    }
}