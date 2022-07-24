using PG.ClassRoom.Model.Data;

namespace PG.ClassRoom.Context.Shop
{
    public partial class ShopMediator
    {
        private class ShopStateDecoration : ShopState
        {
            public ShopStateDecoration(Shop.ShopMediator mediator) : base(mediator)
            {
                
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                ModulesListView.Init(StaticDataModel.MetaData.Modules.FindAll(m => m.ModuleType == EModuleType.Decoration));
            }
        }
    }
}