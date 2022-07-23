using PG.CastleBuilder.Context.Shop.Elements.Friend;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model.Context;
using PG.Core.Installer;
using Zenject;

namespace PG.CastleBuilder.Context.Shop
{
    public class ShopInstaller : ContextInstaller<ShopMediator>
    {
        public ModulesListView ModulesListView;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<ShopModel>().AsSingle();
            Container.Bind<ModulesListView>().FromInstance(ModulesListView).AsSingle();
        }
    }
}