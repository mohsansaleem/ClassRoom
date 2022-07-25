using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using PG.Core.Context;
using Zenject;

namespace PG.ClassRoom.Context.Lobby.Elements.Friend
{
    public class RoomsListView : PagedListView<RoomsListItem, RoomData>
    {
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly RoomsListItem.Pool _friendListItemPool;
        
        protected override RoomsListItem Spawn(RoomData model)
        {
            return _friendListItemPool.Spawn(model);
        }

        protected override void DeSpawn(RoomsListItem itemView)
        {
            _friendListItemPool.Despawn(itemView);
        }

        protected override void Start()
        {
            base.Start();
            //Init(_remoteDataModel.UserData);
        }

        protected override int PageSize => Constants.RoomsListSize;
    }
}