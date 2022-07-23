using UniRx;

namespace PG.CastleBuilder.Model.Context
{
    public class ShopModel
    {
        public readonly ReactiveProperty<EShopState> ShopState;

        public ShopModel()
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

