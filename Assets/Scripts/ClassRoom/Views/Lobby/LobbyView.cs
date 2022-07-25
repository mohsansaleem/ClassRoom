using PG.ClassRoom.Context.Shop.Elements.Friend;
using UnityEngine;
using UnityEngine.UI;

namespace PG.ClassRoom.Views.Gameplay
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