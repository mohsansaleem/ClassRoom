using PG.ClassRoom.Model.Data;

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator
    {
        private class LobbyStateLoading : LobbyState
        {
            public LobbyStateLoading(Lobby.LobbyMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                //RoomsListView.Init(StaticDataModel.MetaData.Rooms.FindAll(m => m.RoomType == ERoomType.Workshop));
            }
        }
    }
}