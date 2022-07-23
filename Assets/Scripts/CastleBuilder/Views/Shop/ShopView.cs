using PG.CastleBuilder.Context.Shop.Elements.Friend;
using UnityEngine;
using UnityEngine.UI;

namespace PG.CastleBuilder.Views.Gameplay
{
    public class ShopView : MonoBehaviour
    {
        public Toggle WorkshopToggle;
        public Toggle DecorationToggle;
        public Button ExitButton;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}