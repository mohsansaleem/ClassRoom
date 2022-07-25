using PG.ClassRoom.Context.Lobby.Elements.Friend;
using UnityEngine;
using UnityEngine.UI;

namespace PG.ClassRoom.Views.Gameplay
{
    public class LobbyView : MonoBehaviour
    {
        public CanvasGroup LoadingPanel;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetLoading(bool isLoading)
        {
            LoadingPanel.alpha = isLoading ? 1 : 0;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}