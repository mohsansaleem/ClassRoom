using PG.CastleBuilder.Context.Shop.Elements.Friend;
using PG.CastleBuilder.DOTS;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Views.Gameplay;
using PG.Core.FSM;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Context.Shop
{
    public partial class ShopMediator : StateMachineMediator
    {
        [Inject] public ProjectContextInstaller.Settings _projectSettings;

        [Inject] private readonly ShopView _view;
        
        [Inject] private readonly BootstrapModel _bootstrapModel;
        [Inject] private readonly ShopModel _shopModel;
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly ModulesListView _modulesListView;

        [Inject] private readonly DOTS_Hub _dotsHub;
        
        public override void Initialize()
        {
            base.Initialize();

            StateBehaviours.Add((int)EShopState.Workshop, new ShopStateWorkshop(this));
            StateBehaviours.Add((int)EShopState.Decorations, new ShopStateDecoration(this));

            _view.ExitButton.OnClickAsObservable().Subscribe(_ =>
            {
                SignalFactory.Create<UnloadSceneSignal>().Unload(Scenes.Shop).Done
                (
                    () =>
                    {
                        Debug.Log("Shop Closed.");
                    }
                );
            }).AddTo(Disposables);

            _view.WorkshopToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                if (isOn)
                {
                    _shopModel.ChangeState(EShopState.Workshop);
                }
            }).AddTo(Disposables);
            
            _view.DecorationToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                _shopModel.ChangeState(EShopState.Decorations);
            }).AddTo(Disposables);
            
            _shopModel.ShopState.Subscribe(OnShopStateChanged).AddTo(Disposables);
        }

        private void OnShopStateChanged(EShopState gamePlayState)
        {
            GoToState((int)gamePlayState);
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}