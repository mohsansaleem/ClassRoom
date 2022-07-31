using System;
using UnityEngine;
using UnityEngine.UI;

namespace PG.ClassRoom.Views.Gameplay
{
    public class GamePlayView : MonoBehaviour
    {
        private void Awake()
        {
            this.transform.position = Vector3.one;
        }

        [Header("Player Data")]
        public Text PlayerName;
        public Text PlayerLevel;
        public Slider PlayerXP;
        public Slider PlayerHappiness;
        public Image HappinessIcon;
        public Text GoldText;

        [Header("Environment")] 
        public Transform GridTransform;
        public Transform Environment;

        [Header("Interaction")]
        public GameObject LowerPanel;
        public Button ShopButton;
        public Button FriendsToggleButton;
        public Button LobbyButton;
        public Text FriendsToggleButtonText;
        public GameObject FriendsPanel;
        public Button ZoomInButton;
        public Button ZoomOutButton;
        
        [Header("Module Selection")]
        public GameObject BuildingInfoPanel;
        public Text BuildingName;
        public Button CollectButton;
        public Button MoveModuleButton;
        public Button RemoveModuleButton;

        // TODO: Move the ErrorPanel to a Separate Facade or Create a generic Dialogue system.
        [Header("Error Panel")]
        public GameObject ErrorPanel;
        public Text ErrorMessageText;
        public Button ErrorPanelOkButton;
        
        // Events
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
        
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}