using System.Threading.Tasks;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Context.Lobby.Elements.Friend;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Service;
using PG.ClassRoom.Views.Gameplay;
using PG.Core.Command;
using PG.Core.FSM;
using PG.Core.Installer;
using Photon.Pun;
using Photon.Realtime;
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
        [Inject] private readonly LobbyContextModel _lobbyContextModel;
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly RealtimeDataModel _realtimeDataModel;
        [Inject] private readonly RoomsListView _modulesListView;
        
        [Inject] private readonly RealtimeHub _realtimeHub;
        
        public override void Initialize()
        {
            base.Initialize();

            InitStates();

            AddListeners();

            _lobbyContextModel.ChangeState(_realtimeHub.InLobby ? ELobbyState.RoomsList : ELobbyState.JoinLobby);
        }

        private void InitStates()
        {
            StateBehaviours.Add((int) ELobbyState.Create, new LobbyStateCreateRoom(this));
            StateBehaviours.Add((int) ELobbyState.RoomsList, new LobbyStateRoomsList(this));
            StateBehaviours.Add((int) ELobbyState.JoinLobby, new LobbyStateRoomsJoinLobby(this));
        }
        
        private void AddListeners()
        {
            _lobbyContextModel.LobbyState.Subscribe(OnLobbyStateChanged).AddTo(Disposables);

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
            _view.JoinButton.onClick.AsObservable().Subscribe(OnJoinButtonClicked).AddTo(Disposables);
            _view.CreateRoomPanelButton.onClick.AsObservable().Subscribe(OnOpenCreatePanelClicked).AddTo(Disposables);
            _view.ExitCreateRoomButton.onClick.AsObservable().Subscribe(OnExitCreateRoomPanelClicked).AddTo(Disposables);
            _view.CreateRoomButton.onClick.AsObservable().Subscribe(OnCreateRoomButtonClicked).AddTo(Disposables);
            _view.ErrorOkButton.onClick.AsObservable().Subscribe(OnErrorMessageOkClicked).AddTo(Disposables);

            _realtimeDataModel.CurrentRoom.Subscribe(OnRoomJoined).AddTo(Disposables);
        }

        private void OnRoomJoined(Room room)
        {
            if (room != null)
            {
                _view.Hide();
                SignalFactory.Create<LoadSceneSignal>().Load(Scenes.GamePlay);
            }
            else
            {
                _view.Show();
            }
        }

        private void OnJoinButtonClicked(Unit _)
        {
            _realtimeHub.Connect(_view.UserNameInputField.text).Then(
                async (isConnectSuccess) =>
                {
                    if (isConnectSuccess)
                    {
                        _realtimeHub.JoinLobby().Then(
                            async (isSuccess) =>
                            {
                                if (isSuccess)
                                {
                                    _lobbyContextModel.ChangeState(ELobbyState.RoomsList);
                                }
                                else
                                {
                                    _view.ShowErrorMessage("Unable to join the default lobby");
                                    Debug.LogError("Unable to join the default lobby");
                                }
                            }
                            , e =>
                            {
                                _view.ShowErrorMessage("Unable to join the default lobby");
                                Debug.LogError("Unable to join the default lobby");
                            });
                    }
                    else
                    {
                        _view.ShowErrorMessage("Unable to connect to photon");
                        Debug.LogError("Unable to connect to photon");
                    }
                }
                , e =>
                {
                    Debug.LogError("Unable to connect to photon");
                });
        }
        
        private void OnOpenCreatePanelClicked(Unit _)
        {
            _lobbyContextModel.ChangeState(ELobbyState.Create);
        }

        private void OnExitCreateRoomPanelClicked(Unit _)
        {
            _lobbyContextModel.ChangeState(ELobbyState.RoomsList);
        }

        private void OnCreateRoomButtonClicked(Unit _)
        {
            // Create Room
            string roomName = _view.RoomName.text;

            RoomOptions options = new RoomOptions {PlayerTtl = 10000};
            if (!_realtimeHub.CreateRoom(roomName, options))
            {
                _view.ShowErrorMessage("Unable to create room");
            }
        }

        private void OnErrorMessageOkClicked(Unit _)
        {
            _view.HideErrorMessage();
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