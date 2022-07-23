using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
{
    public partial class BootstrapMediator
    {
        private class BootstrapStateCreateUserData : BootstrapState
        {
            private readonly RemoteDataModel _remoteDataModel;

            public BootstrapStateCreateUserData(Bootstrap.BootstrapMediator mediator) : base(mediator)
            {
                _remoteDataModel = mediator._remoteDataModel;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                UserData userData = GameSettings.DefaultGameState.User;

                SignalFactory.Create<CreateUserDataSignal>().CreateUserData(userData).Then(
                    () => {
                        BootstrapModel.ChangeState(Model.Context.BootstrapModel.ELoadingProgress.LoadUserData);
                    }
                    ,e =>
                    {
                        Debug.LogError("Exception Creating new User: " + e);
                    });
            }
        }
    }
}