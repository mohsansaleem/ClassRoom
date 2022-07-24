using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using UniRx;
using UnityEngine;

namespace PG.ClassRoom.Context.Gameplay
{
    public partial class GamePlayMediator
    {
        private class GamePlayStateModuleMenu : GamePlayState
        {
            public GamePlayStateModuleMenu(GamePlayMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                
                View.BuildingInfoPanel.SetActive(true);
                
                View.CollectButton.onClick.AsObservable().Subscribe(_ =>
                {
                    Debug.LogError("Collect Resource is not Implemented.");
                }).AddTo(Disposables);
                
                View.MoveModuleButton.onClick.AsObservable().Subscribe(_ =>
                {
                    Debug.LogError("Move Module is not Implemented.");
                }).AddTo(Disposables);
                
                View.RemoveModuleButton.onClick.AsObservable().Subscribe(_ =>
                {
                    SignalFactory.Create<RemoveModuleSignal>()
                        .RemoveModule(GamePlayContextModel.SelectedModule.Value.RemoteData.RemoteId).Done(
                            () => { GamePlayContextModel.ChangeState(EGamePlayState.Regular); },
                            exception => { Debug.LogError("Something went wrong while removing Module."); });
                }).AddTo(Disposables);

                GamePlayContextModel.SelectedModule.Subscribe(OnModuleSelected).AddTo(Disposables);
            }

            private void OnClickAtModule(ModuleRemoteDataModel model)
            {
                GamePlayContextModel.SelectedModule.Value = model;
            }
            
            private void OnModuleSelected(ModuleRemoteDataModel model)
            {
                if (model != null)
                {
                    View.BuildingName.text = $"{model.Data.ModuleName}";
                    
                    View.CollectButton.gameObject.SetActive(model.CanProduce);
                }
                else
                {
                    GamePlayContextModel.ChangeState(EGamePlayState.Regular);
                }
            }

            public override void OnStateExit()
            {
                base.OnStateExit();
                
                View.BuildingInfoPanel.SetActive(false);
            }
        }
    }
}