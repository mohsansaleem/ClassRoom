using UniRx;

namespace PG.CastleBuilder.Model.Context
{
    public class BootstrapModel
    {
        public enum ELoadingProgress
        {
            NotLoaded = -1,
            LoadStaticData = 0,
            CreateMetaData = 30,
            LoadUserData = 50,
            CreateUserData = 70,
            GamePlay = 100
        }
        
        public readonly ReactiveProperty<ELoadingProgress> LoadingProgress;

        public BootstrapModel()
        {
            LoadingProgress = new ReactiveProperty<ELoadingProgress>(ELoadingProgress.LoadStaticData);
        }

        public void ChangeState(ELoadingProgress state)
        {
            LoadingProgress.Value = state;
        }
    }
    
    
}

