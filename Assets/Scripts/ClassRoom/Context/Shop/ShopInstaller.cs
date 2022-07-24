using PG.ClassRoom.Installer;
using PG.ClassRoom.Context.Shop.Elements.Friend;
using PG.ClassRoom.Model.Context;
using PG.Core.Installer;
using Zenject;

namespace PG.ClassRoom.Context.Shop
{
    public class ShopInstaller : ContextInstaller<ShopMediator>
    {
        public ModulesListView ModulesListView;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<ShopContextModel>().AsSingle();
            Container.Bind<ModulesListView>().FromInstance(ModulesListView).AsSingle();
        }
    }
}