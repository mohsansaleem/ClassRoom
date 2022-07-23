using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Views.Bootstrap;
using PG.Core.FSM;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Context.Bootstrap
{
    public partial class BootstrapMediator : StateMachineMediator
    {
        [Inject] private readonly BootstrapView _view;
        
        [Inject] private readonly BootstrapModel _bootstrapModel;

        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        [Inject] private ProjectContextInstaller.Settings _gameSettings;

        public BootstrapMediator()
        {
            Disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();

            StateBehaviours.Add((int)BootstrapModel.ELoadingProgress.LoadStaticData, new BootstrapStateLoadStaticData(this));
            StateBehaviours.Add((int)BootstrapModel.ELoadingProgress.CreateMetaData, new BootstrapStateCreateMetaData(this)); // Temporary State for creating MetaData
            StateBehaviours.Add((int)BootstrapModel.ELoadingProgress.LoadUserData, new BootstrapStateLoadUserData(this));
            StateBehaviours.Add((int)BootstrapModel.ELoadingProgress.CreateUserData, new BootstrapStateCreateUserData(this));
            StateBehaviours.Add((int)BootstrapModel.ELoadingProgress.GamePlay, new BootstrapStateGamePlay(this));

            _bootstrapModel.LoadingProgress.Subscribe(OnLoadingProgressChanged).AddTo(Disposables);
        }

        private void OnLoadingProgressChanged(BootstrapModel.ELoadingProgress loadingProgress)
        {
            GoToState((int)loadingProgress);
            
            _view.ProgressBar.value = (float)loadingProgress / 100;
        }

        private void OnReload()
        {
            _view.Show();

            SignalFactory.Create<UnloadAllScenesExceptSignal>().UnloadAllExcept(Scenes.Bootstrap).Done
            (
                () =>
                {
                    _bootstrapModel.ChangeState(BootstrapModel.ELoadingProgress.LoadStaticData);
                },
                exception =>
                {
                    Debug.LogError("Error While Reloading: " + exception);
                }
            );
        }

        private void OnLoadingStart()
        {
            _view.Show();
        }
    }
}

