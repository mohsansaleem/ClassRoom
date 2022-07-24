using System.Threading.Tasks;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
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
                    async () =>
                    {
                        // For progress animation effect
                        await Task.Delay(200);
                        BootstrapContextModel.ChangeState(Model.Context.BootstrapContextModel.ELoadingProgress.LoadUserData);
                    }
                    ,e =>
                    {
                        Debug.LogError("Exception Creating new User: " + e);
                    });
            }
        }
    }
}