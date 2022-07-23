using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.Core.Context;
using Zenject;

namespace PG.CastleBuilder.Context.Gameplay
{
    public class FriendsListView : PagedListView<FriendListItem, FriendData>
    {
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly FriendListItem.Pool _friendListItemPool;
        
        protected override FriendListItem Spawn(FriendData model)
        {
            return _friendListItemPool.Spawn(model);
        }

        protected override void DeSpawn(FriendListItem itemView)
        {
            _friendListItemPool.Despawn(itemView);
        }

        protected override void Start()
        {
            base.Start();
            Init(_remoteDataModel.UserData.Friends);
        }

        protected override int PageSize => Constants.FriendsListSize;
    }
}