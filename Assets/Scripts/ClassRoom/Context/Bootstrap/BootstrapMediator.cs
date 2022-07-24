using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Bootstrap;
using PG.Core.FSM;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Context.Bootstrap
{
    public partial class BootstrapMediator : StateMachineMediator
    {
        [Inject] private readonly BootstrapView _view;
        
        [Inject] private readonly BootstrapContextModel _bootstrapContextModel;

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

            StateBehaviours.Add((int)BootstrapContextModel.ELoadingProgress.LoadStaticData, new BootstrapStateLoadStaticData(this));
            StateBehaviours.Add((int)BootstrapContextModel.ELoadingProgress.CreateMetaData, new BootstrapStateCreateMetaData(this)); // Temporary State for creating MetaData
            StateBehaviours.Add((int)BootstrapContextModel.ELoadingProgress.LoadUserData, new BootstrapStateLoadUserData(this));
            StateBehaviours.Add((int)BootstrapContextModel.ELoadingProgress.CreateUserData, new BootstrapStateCreateUserData(this));
            StateBehaviours.Add((int)BootstrapContextModel.ELoadingProgress.GamePlay, new BootstrapStateGamePlay(this));

            _bootstrapContextModel.LoadingProgress.Subscribe(OnLoadingProgressChanged).AddTo(Disposables);
        }

        private void OnLoadingProgressChanged(BootstrapContextModel.ELoadingProgress loadingProgress)
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
                    _bootstrapContextModel.ChangeState(BootstrapContextModel.ELoadingProgress.LoadStaticData);
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

