using PG.CastleBuilder.Context.Shop.Elements.Friend;
using PG.CastleBuilder.DOTS;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Views.Gameplay;
using PG.Core.FSM;

namespace PG.CastleBuilder.Context.Shop
{
    public partial class ShopMediator
    {
        private class ShopState : StateMachineMediator.StateBehaviour
        {
            protected readonly Shop.ShopMediator Mediator;
            
            protected readonly ShopModel ShopModel;
            protected readonly ShopView View;

            protected readonly ProjectContextInstaller.Settings ProjectSettings;

            protected readonly RemoteDataModel RemoteDataModel;
            protected readonly StaticDataModel StaticDataModel;

            protected readonly ModulesListView ModulesListView;

            protected readonly DOTS_Hub DOTS_Hub;

            public ShopState(Shop.ShopMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                
                this.ShopModel = mediator._shopModel;
                this.View = mediator._view;

                this.ProjectSettings = mediator._projectSettings;

                this.RemoteDataModel = mediator._remoteDataModel;
                this.StaticDataModel = mediator._staticDataModel;

                this.ModulesListView = mediator._modulesListView;
                
                this.DOTS_Hub = mediator._dotsHub;
            }
        }
    }
}
