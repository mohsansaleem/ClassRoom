using PG.ClassRoom.Context.Shop.Elements.Friend;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Views.Gameplay;
using PG.Core.FSM;

namespace PG.ClassRoom.Context.Shop
{
    public partial class ShopMediator
    {
        private class ShopState : StateMachineMediator.StateBehaviour
        {
            protected readonly Shop.ShopMediator Mediator;
            
            protected readonly ShopContextModel ShopContextModel;
            protected readonly ShopView View;

            protected readonly ProjectContextInstaller.Settings ProjectSettings;

            protected readonly RemoteDataModel RemoteDataModel;
            protected readonly StaticDataModel StaticDataModel;

            protected readonly ModulesListView ModulesListView;

            public ShopState(Shop.ShopMediator mediator) : base(mediator)
            {
                this.Mediator = mediator;
                
                this.ShopContextModel = mediator._shopContextModel;
                this.View = mediator._view;

                this.ProjectSettings = mediator._projectSettings;

                this.RemoteDataModel = mediator._remoteDataModel;
                this.StaticDataModel = mediator._staticDataModel;

                this.ModulesListView = mediator._modulesListView;
            }
        }
    }
}
