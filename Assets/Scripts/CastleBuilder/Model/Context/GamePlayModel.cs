using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using UniRx;

namespace PG.CastleBuilder.Model.Context
{
    public class GamePlayModel
    {
        public readonly ReactiveProperty<EGamePlayState> GamePlayState;
        public readonly ReactiveProperty<ModuleRemoteDataModel> SelectedModule;
        public readonly ReactiveProperty<ModuleData> ModuleToAttach;

        public GamePlayModel()
        {
            GamePlayState = new ReactiveProperty<EGamePlayState>(EGamePlayState.Load);
            SelectedModule = new ReactiveProperty<ModuleRemoteDataModel>(null);
            ModuleToAttach = new ReactiveProperty<ModuleData>();
        }
        
        public void ChangeState(EGamePlayState state)
        {
            GamePlayState.Value = state;
        }

        public void ChangeStateAndNotify(EGamePlayState state)
        {
            GamePlayState.SetValueAndForceNotify(state);
        }
    }

    public enum EGamePlayState
    {
        Load = 0,
        Regular,
        PlaceModule,
        ModuleMenu,
        MoveModule
    }
}

