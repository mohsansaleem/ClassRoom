using PG.ClassRoom.Model.Data;

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator
    {
        private class LobbyStateCreateRoom : LobbyState
        {
            public LobbyStateCreateRoom(Lobby.LobbyMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                View.ShowCreateRoom();
                //RoomsListView.Init(StaticDataModel.MetaData.Rooms.FindAll(m => m.RoomType == ERoomType.Workshop));
            }

            public override void OnStateExit()
            {
                View.HideCreateRoom();
                base.OnStateExit();
            }
        }
    }
}