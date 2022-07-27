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
        
        private Promise<bool> _connetionPromise;
        public IPromise<bool> Connect()
        {
            var promiseReturn = new Promise<bool>();

            if (!IsConnected)
            {
                _connetionPromise = promiseReturn;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _staticDataModel.MetaData.RealtimeGameVersion;
            }

            if (IsConnected)
            {
                promiseReturn.Resolve(true);
                _connetionPromise = null;
            }

            return promiseReturn;
        }

        public override void OnConnected()
        {
            base.OnConnected();
            _connetionPromise?.Resolve(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            _connetionPromise?.Resolve(false);
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
    }
}