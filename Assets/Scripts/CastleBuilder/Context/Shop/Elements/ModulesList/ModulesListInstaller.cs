using PG.Core.Installer;
using UnityEngine;

namespace PG.CastleBuilder.Context.Shop.Elements.Friend
{
    public class ModulesListInstaller : ContextInstaller<ModulesListView>
    {
        public ModulesListView ModulesListView;
        public Transform ModulesListItemsContainer;
        public ModulesListItem ModulesListItemPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ModulesListView>().FromInstance(ModulesListView).AsSingle();

            Container.BindMemoryPool<ModulesListItem, ModulesListItem.Pool>()
                .WithInitialSize(Constants.ModulesListSize).WithMaxSize(Constants.ModulesListSize)
                .FromComponentInNewPrefab(ModulesListItemPrefab)
                .UnderTransform(ModulesListItemsContainer);
        }
    }
}