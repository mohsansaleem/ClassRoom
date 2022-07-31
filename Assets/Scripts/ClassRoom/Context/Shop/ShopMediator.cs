using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Context.Shop.Elements.Friend;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Gameplay;
using PG.Core.FSM;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Context.Shop
{
    public partial class ShopMediator : StateMachineMediator
    {
        [Inject] public ProjectContextInstaller.Settings _projectSettings;

        [Inject] private readonly ShopView _view;
        
        [Inject] private readonly BootstrapContextModel _bootstrapContextModel;
        [Inject] private readonly ShopContextModel _shopContextModel;
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly ModulesListView _modulesListView;
        
        public override void Initialize()
        {
            base.Initialize();

            InitStates();
            AddListeners();
        }

        private void InitStates()
        {
            StateBehaviours.Add((int) EShopState.Workshop, new ShopStateWorkshop(this));
            StateBehaviours.Add((int) EShopState.Decorations, new ShopStateDecoration(this));
        }

        private void AddListeners()
        {
            _view.ExitButton.OnClickAsObservable().Subscribe(OnExitButtonClicked).AddTo(Disposables);

            _view.WorkshopToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                if (isOn)
                {
                    _shopContextModel.ChangeState(EShopState.Workshop);
                }
            }).AddTo(Disposables);

            _view.DecorationToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                _shopContextModel.ChangeState(EShopState.Decorations);
            }).AddTo(Disposables);

            _shopContextModel.ShopState.Subscribe(OnShopStateChanged).AddTo(Disposables);
        }

        private void OnExitButtonClicked(Unit _)
        {
            SignalFactory.Create<UnloadSceneSignal>().Unload(Scenes.Shop).Done(() => { Debug.Log("Shop Closed."); });
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