using UniRx;

namespace PG.ClassRoom.Model.Context
{
    public class BootstrapContextModel
    {
        public enum ELoadingProgress
        {
            NotLoaded = -1,
            LoadStaticData = 0,
            CreateMetaData = 20,
            LoadUserData = 40,
            CreateUserData = 60,
            RealTimeServer = 80,
            Lobby = 100,
        }
        
        public readonly ReactiveProperty<ELoadingProgress> LoadingProgress;

        public BootstrapContextModel()
        {
            LoadingProgress = new ReactiveProperty<ELoadingProgress>(ELoadingProgress.LoadStaticData);
        }

        public void ChangeState(ELoadingProgress state)
        {
            LoadingProgress.Value = state;
        }
    }
    
    
}

