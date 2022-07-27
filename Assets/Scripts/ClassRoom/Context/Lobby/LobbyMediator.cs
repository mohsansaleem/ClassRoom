using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Context.Lobby.Elements.Friend;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Gameplay;
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
        
        
        public override void Initialize()
        {
            base.Initialize();

            StateBehaviours.Add((int)ELobbyState.Create, new LobbyStateCreateRoom(this));
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

            _lobbyContextModel.LobbyState.Subscribe(OnLobbyStateChanged).AddTo(Disposables);
            
            _view.CreateRoomPanelButton.onClick.AsObservable().Subscribe(_ =>
                {
                    _lobbyContextModel.LobbyState.Value = ELobbyState.Create;
                }).AddTo(Disposables);
            _view.ExitCreateRoomButton.onClick.AsObservable().Subscribe(_ =>
            {
                _lobbyContextModel.LobbyState.Value = ELobbyState.RoomsList;
            }).AddTo(Disposables);
            _view.CreateRoomButton.onClick.AsObservable().Subscribe(_ =>
            {
                // Create Room
                string roomName = _view.RoomName.text;
                
                RoomOptions options = new RoomOptions {PlayerTtl = 10000 };
                if (PhotonNetwork.CreateRoom(roomName, options, null))
                {
                    
                }
                else
                {
                    _view.ShowErrorMessage("Unable to create room");
                }
                
            }).AddTo(Disposables);
            _view.ErrorOkButton.onClick.AsObservable().Subscribe(_ =>
            {
                _view.HideErrorMessage();
            }).AddTo(Disposables);

            _lobbyContextModel.LobbyState.Value = ELobbyState.RoomsList;
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