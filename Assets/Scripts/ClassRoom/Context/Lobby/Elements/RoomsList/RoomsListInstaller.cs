using PG.Core.Installer;
using UnityEngine;

namespace PG.ClassRoom.Context.Lobby.Elements.Friend
{
    public class RoomsListInstaller : ContextInstaller<RoomsListView>
    {
        public RoomsListView RoomsListView;
        public Transform RoomsListItemsContainer;
        public RoomsListItem RoomsListItemPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<RoomsListView>().FromInstance(RoomsListView).AsSingle();

            Container.BindMemoryPool<RoomsListItem, RoomsListItem.Pool>()
                .WithInitialSize(Constants.RoomsListSize).WithMaxSize(Constants.RoomsListSize)
                .FromComponentInNewPrefab(RoomsListItemPrefab)
                .UnderTransform(RoomsListItemsContainer);
        }
    }
}