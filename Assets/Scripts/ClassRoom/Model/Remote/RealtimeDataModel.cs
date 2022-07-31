using System.Linq;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model.Data;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Model.Remote
{
    public class RealtimeDataModel
    {
        [Inject] private ProjectContextInstaller.Settings _settings;

        public readonly ReactiveDictionary<string, RoomInfo> RoomsRemoteDatas;
        public readonly ReactiveProperty<Room> CurrentRoom;

        public RealtimeDataModel()
        {
            RoomsRemoteDatas = new ReactiveDictionary<string, RoomInfo>();
            CurrentRoom = new ReactiveProperty<Room>();
        }

        public void SetRoom(Room room)
        {
            CurrentRoom.Value = room;
        }
        
        public void ClearRooms()
        {
            RoomsRemoteDatas.Clear();
        }
    }
}

