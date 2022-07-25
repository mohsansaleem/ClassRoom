using UniRx;

namespace PG.ClassRoom.Model.Context
{
    public class LobbyContextModel
    {
        public readonly ReactiveProperty<ELobbyState> LobbyState;

        public LobbyContextModel()
        {
            LobbyState = new ReactiveProperty<ELobbyState>(ELobbyState.Loading);
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
        Loading = 0,
        RoomsList
    }
}

