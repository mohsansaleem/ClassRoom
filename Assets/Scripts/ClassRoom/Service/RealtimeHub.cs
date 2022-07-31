using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PG.ClassRoom.Model;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Model.Response;
using Photon.Pun;
using Photon.Realtime;
using RSG;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Service
{
    public class RealtimeHub : MonoBehaviourPunCallbacks, IRealtimeHub
    {
        [Inject] private StaticDataModel _staticDataModel;
        [Inject] private RealtimeDataModel _realtimeDataModel;
        
        public bool IsConnected => PhotonNetwork.IsConnected;
        public bool InLobby => PhotonNetwork.InLobby;
        public bool InRoom => PhotonNetwork.InRoom;
        
        private Promise<bool> _connetionPromise;
        private Promise<bool> _lobbyJoinPromise;
        
        public IPromise<bool> Connect(string userId)
        {
            var promiseReturn = new Promise<bool>();

            if (IsConnected)
            {
                promiseReturn.Resolve(true);
                _connetionPromise = null;
            }
            
            if (!IsConnected)
            {
                _connetionPromise = promiseReturn;
                PhotonNetwork.AuthValues = new AuthenticationValues {UserId = userId};
                PhotonNetwork.ConnectUsingSettings();
            }

            return promiseReturn;
        }

        public IPromise<bool> JoinLobby()
        {
            var promiseReturn = new Promise<bool>();
            
            if (!InLobby)
            {
                _lobbyJoinPromise = promiseReturn;
                PhotonNetwork.JoinLobby();
            }

            if (InLobby)
            {
                promiseReturn.Resolve(true);
                _lobbyJoinPromise = null;
            }

            return promiseReturn;
        }

        public bool CreateRoom(string roomName)
        {
            return PhotonNetwork.CreateRoom(roomName);
        }
        
        public bool JoinRoom(string roomName)
        {
            return PhotonNetwork.JoinRoom(roomName);
        }

        public bool LeaveRoom()
        {
            return PhotonNetwork.LeaveRoom();
        }
        
        #region Events
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            _connetionPromise?.Resolve(true);
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            _lobbyJoinPromise?.Resolve(true);
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            _lobbyJoinPromise?.Resolve(false);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            
            if (roomList.Count == 0 && !PhotonNetwork.InLobby)
            {
                _realtimeDataModel.ClearRooms();
            }
		
            foreach (RoomInfo entry in roomList)
            {
                if (_realtimeDataModel.RoomsRemoteDatas.ContainsKey(entry.Name))
                {
                    if (entry.RemovedFromList)
                    {
                        _realtimeDataModel.RoomsRemoteDatas.Remove(entry.Name);
                    }
                    else
                    {
                        // we update the cell
                        _realtimeDataModel.RoomsRemoteDatas[entry.Name] = entry;
                    }

                }
                else
                {
                    if (!entry.RemovedFromList)
                    {
                        // we create the cell
                        _realtimeDataModel.RoomsRemoteDatas.Add(entry.Name, entry);
                    }
                }
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _realtimeDataModel.SetRoom(InRoom ? PhotonNetwork.CurrentRoom : null);
        }
        
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            _realtimeDataModel.SetRoom(null);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            _connetionPromise?.Reject(new Exception("Unable to connect"));
        }
        #endregion Events
    }
}