using UniRx;

namespace PG.ClassRoom.Model.Context
{
    public class LobbyContextModel
    {
        public readonly ReactiveProperty<ELobbyState> LobbyState;

        public LobbyContextModel()
        {
            LobbyState = new ReactiveProperty<ELobbyState>(ELobbyState.Create);
        }
        
        public void ChangeState(ELobbyState state)
        {
            LobbyState.Value = state;
        }

        public void ChangeStateAndNotify(ELobbyState state)
        {
            LobbyState.SetValueAndForceNotify(state);
        }
    }

    public enum ELobbyState
    {
        Create = 0,
        RoomsList,
        JoinLobby
    }
}

