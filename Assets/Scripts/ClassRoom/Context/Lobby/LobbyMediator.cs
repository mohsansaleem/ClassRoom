using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Context.Lobby.Elements.Friend;
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

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator : StateMachineMediator
    {
        [Inject] public ProjectContextInstaller.Settings _projectSettings;

        [Inject] private readonly LobbyView _view;
        
        [Inject] private readonly BootstrapContextModel _bootstrapContextModel;
        [Inject] private readonly LobbyContextModel _shopContextModel;
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly RoomsListView _modulesListView;
        
        public override void Initialize()
        {
            base.Initialize();

            StateBehaviours.Add((int)ELobbyState.Loading, new LobbyStateLoading(this));
            StateBehaviours.Add((int)ELobbyState.RoomsList, new LobbyStateRoomsList(this));

            /*_view.ExitButton.OnClickAsObservable().Subscribe(_ =>
            {
                SignalFactory.Create<UnloadSceneSignal>().Unload(Scenes.Lobby).Done
                (
                    () =>
                    {
                        Debug.Log("Lobby Closed.");
                    }
                );
            }).AddTo(Disposables);*/

            _shopContextModel.LobbyState.Subscribe(OnLobbyStateChanged).AddTo(Disposables);
        }

        private void OnLobbyStateChanged(ELobbyState gamePlayState)
        {
            GoToState((int)gamePlayState);
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}