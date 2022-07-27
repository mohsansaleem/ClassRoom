using System;
using System.Collections.Generic;
using System.Linq;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using Photon.Realtime;
using UniRx;

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator
    {
        private class LobbyStateRoomsList : LobbyState
        {
            private Dictionary<string, RoomData> roomsCache = new Dictionary<string, RoomData>();

            public LobbyStateRoomsList(Lobby.LobbyMediator mediator) : base(mediator)
            {
            }

            private void OnRoomAdd(DictionaryAddEvent<string, RoomInfo> roomInfo)
            {
                if (!roomsCache.ContainsKey(roomInfo.Key))
                {
                    RoomData roomData = GetRoomData(roomInfo.Value);
                    roomsCache[roomInfo.Key] = roomData;
                    RoomsListView.AddItem(roomData);
                }
                
                View.RoomsCount.text = roomsCache.Count.ToString();
            }
            
            private void OnRoomRemove(DictionaryRemoveEvent<string, RoomInfo> roomInfo)
            {
                if (roomsCache.ContainsKey(roomInfo.Key))
                {
                    RoomsListView.RemoveItem(roomsCache[roomInfo.Key]);
                    roomsCache.Remove(roomInfo.Key);
                }
                
                View.RoomsCount.text = roomsCache.Count.ToString();
            }
            
            private void OnRoomUpdate(DictionaryReplaceEvent<string, RoomInfo> roomInfo)
            {
                if (roomsCache.ContainsKey(roomInfo.Key))
                {
                    RoomData roomData = GetRoomData(roomInfo.NewValue);
                    int indexOfItem = RoomsListView.GetIndexOfItem(roomsCache[roomInfo.Key]);
                    RoomsListView.ReplaceItem(roomData, indexOfItem);
                }
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                foreach (KeyValuePair<string,RoomInfo> roomsRemoteData in RealtimeDataModel.RoomsRemoteDatas)
                {
                    if (!roomsCache.ContainsKey(roomsRemoteData.Key))
                    {
                        roomsCache[roomsRemoteData.Key] = GetRoomData(roomsRemoteData.Value);
                    }
                }
                
                
                List<string> roomsToRemove = new List<string>();
                foreach (string key in roomsCache.Keys)
                {
                    if (!RealtimeDataModel.RoomsRemoteDatas.ContainsKey(key))
                        roomsToRemove.Add(key);
                }

                foreach (string roomName in roomsToRemove)
                {
                    roomsCache.Remove(roomName);
                }
                
                
                RoomsListView.Init(roomsCache.Values.ToList());
                
                AddListener();

                View.RoomsCount.text = roomsCache.Count.ToString();
            }

            private void AddListener()
            {
                RealtimeDataModel.RoomsRemoteDatas.ObserveAdd().Subscribe(OnRoomAdd).AddTo(Disposables);
                RealtimeDataModel.RoomsRemoteDatas.ObserveRemove().Subscribe(OnRoomRemove).AddTo(Disposables);
                RealtimeDataModel.RoomsRemoteDatas.ObserveReplace().Subscribe(OnRoomUpdate).AddTo(Disposables);
            }

            private RoomData GetRoomData(RoomInfo roomInfo)
            {
                return new RoomData()
                {
                    RoomName = roomInfo.Name,
                    MembersCount = roomInfo.PlayerCount.ToString()
                };
            }
        }
    }
}