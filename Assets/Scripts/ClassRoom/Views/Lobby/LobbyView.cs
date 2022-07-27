using PG.ClassRoom.Context.Lobby.Elements.Friend;
using UnityEngine;
using UnityEngine.UI;

namespace PG.ClassRoom.Views.Gameplay
{
    public class LobbyView : MonoBehaviour
    {
        public CanvasGroup ErrorPanel;
        public Text ErrorMessage;
        public Button ErrorOkButton;
        public CanvasGroup CreatePanel;
        public Text RoomsCount;
        public Button CreateRoomPanelButton;
        public InputField RoomName;
        public Button CreateRoomButton;
        public Button ExitCreateRoomButton;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void ShowErrorMessage(string message)
        {
            ErrorMessage.text = message;
            ErrorPanel.gameObject.SetActive(true);
        }

        public void HideErrorMessage()
        {
            ErrorPanel.gameObject.SetActive(false);
        }

        public void ShowCreateRoom()
        {
            CreatePanel.gameObject.SetActive(true);
        }

        
        public void HideCreateRoom()
        {
            CreatePanel.gameObject.SetActive(false);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}