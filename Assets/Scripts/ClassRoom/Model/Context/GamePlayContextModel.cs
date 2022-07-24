using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using UniRx;

namespace PG.ClassRoom.Model.Context
{
    public class GamePlayContextModel
    {
        public readonly ReactiveProperty<EGamePlayState> GamePlayState;
        public readonly ReactiveProperty<ModuleRemoteDataModel> SelectedModule;
        public readonly ReactiveProperty<ModuleData> ModuleToAttach;

        public GamePlayContextModel()
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

