using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using PG.Core.Context;
using Zenject;

namespace PG.ClassRoom.Context.Shop.Elements.Friend
{
    public class ModulesListView : PagedListView<ModulesListItem, ModuleData>
    {
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly ModulesListItem.Pool _friendListItemPool;
        
        protected override ModulesListItem Spawn(ModuleData model)
        {
            return _friendListItemPool.Spawn(model);
        }

        protected override void DeSpawn(ModulesListItem itemView)
        {
            _friendListItemPool.Despawn(itemView);
        }

        protected override void Start()
        {
            base.Start();
            //Init(_remoteDataModel.UserData);
        }

        protected override int PageSize => Constants.ModulesListSize;
    }
}