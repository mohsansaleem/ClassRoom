using PG.Core.Installer;
using UnityEngine;

namespace PG.ClassRoom.Context.Gameplay
{
    public class FriendsListInstaller : ContextInstaller<FriendsListView>
    {
        public FriendsListView FriendsListView;
        public Transform FriendListItemsContainer;
        public FriendListItem FriendListItemPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<FriendsListView>().FromInstance(FriendsListView).AsSingle();

            Container.BindMemoryPool<FriendListItem, FriendListItem.Pool>()
                .WithInitialSize(Constants.FriendsListSize).WithMaxSize(Constants.FriendsListSize)
                .FromComponentInNewPrefab(FriendListItemPrefab)
                .UnderTransform(FriendListItemsContainer);
        }
    }
}