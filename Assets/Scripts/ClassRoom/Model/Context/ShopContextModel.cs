using UniRx;

namespace PG.ClassRoom.Model.Context
{
    public class ShopContextModel
    {
        public readonly ReactiveProperty<EShopState> ShopState;

        public ShopContextModel()
        {
            ShopState = new ReactiveProperty<EShopState>(EShopState.Workshop);
        }
        
        public void ChangeState(EShopState state)
        {
            ShopState.Value = state;
        }

        public void ChangeStateAndNotify(EShopState state)
        {
            ShopState.SetValueAndForceNotify(state);
        }
    }

    public enum EShopState
    {
        Workshop = 0,
        Decorations
    }
}

