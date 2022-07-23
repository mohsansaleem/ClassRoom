using PG.CastleBuilder.Service;
using Zenject;

namespace PG.Core.Command
{
    public abstract class RemoteCommand : BaseCommand
    {
        [Inject] protected readonly IRemoteDataService Service;
    }
}