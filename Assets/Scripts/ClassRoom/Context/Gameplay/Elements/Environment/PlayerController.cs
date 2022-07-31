using System;
using PG.ClassRoom.Model;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace PG.ClassRoom.Context.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        public CharacterController _characterController;
        public GameObject PlayerFollowCamera;
        public PlayerInput PlayerInput;
        public PhotonView PhotonView;

        private void Awake()
        {
            if (!PhotonView.IsMine)
            {
                PlayerFollowCamera.gameObject.SetActive(false);
                _characterController.enabled = false;
                PlayerInput.enabled = false;
            }
        }

        public class Factory : PlaceholderFactory<PlayerController>
        {
        }
    }
}
